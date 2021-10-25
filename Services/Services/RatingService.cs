using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Services;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using DAL.Entities.Ratings;
using DAL.Entities.Ratings.enums;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Rating.Inputs;
using Model.Rating.Outputs;

namespace Services
{
    public class RatingService : IRatingService
    {
        private readonly IGenericRepository<Rating> _ratingRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IMapper _mapper;

        public RatingService(IGenericRepository<Rating> ratingRepository, IGenericRepository<Course> courseRepository,
         IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<ResultService<RatingOutput>> GetRatingForCourseAsync(int id)
        {
            var result = new ResultService<RatingOutput>();
            try
            {
                var dbRecordRating = await _ratingRepository.GetQuery().Include(c => c.Course).Include(us => us.User)
                    .Where(x => x.CourseId == id).ToListAsync();

                if (dbRecordRating.Count == 0)
                {
                    result.Code = ResultStatusCode.NotFound;
                    result.Messege = "Course not found";
                    return result;
                }

                var ratingCourse = new RatingOutput();
                foreach (var rating in dbRecordRating)
                {
                    switch (rating.RatingStar)
                    {
                        case RatingStar.One:
                            ratingCourse.NumberOneStars++;
                            break;
                        case RatingStar.Two:
                            ratingCourse.NumberTwoStars++;
                            break;
                        case RatingStar.Three:
                            ratingCourse.NumberThreeStars++;
                            break;
                        case RatingStar.Four:
                            ratingCourse.NumberFourStars++;
                            break;
                        case RatingStar.Five:
                            ratingCourse.NumberFiveStars++;
                            break;
                    }
                }
                long count = ratingCourse.NumberOneStars + ratingCourse.NumberTwoStars + ratingCourse.NumberThreeStars +
                    ratingCourse.NumberFourStars + ratingCourse.NumberFiveStars;
                long sum = ratingCourse.NumberOneStars + (ratingCourse.NumberTwoStars * 2) + (ratingCourse.NumberThreeStars * 3) +
                    (ratingCourse.NumberFourStars * 4) + (ratingCourse.NumberFiveStars * 5);

                ratingCourse.NumberOfRating = count;
                ratingCourse.Rate = count == 0 ? 0.0 : sum / (count * 1.0);
                ratingCourse.CourseId = id;
                ratingCourse.CourseName = dbRecordRating.Select(x => x.Course.Name).FirstOrDefault();

                result.Code = ResultStatusCode.Ok;
                result.Result = ratingCourse;
                result.Messege = "Success";
                return result;
            }
            catch
            {
                result.Code = ResultStatusCode.InternalServerError;
                result.Messege = "Exception happen when get rating for course";
                return result;
            }
        }


        public async Task<ResultService<bool>> RatingCourseAsync(RatingInput input, User user)
        {
            var result = new ResultService<bool>();
            try
            {
                if (user == null)
                {
                    result.Code = ResultStatusCode.Unauthorized;
                    result.Messege = "You are Unauthorized";
                    result.Result = false;
                    return result;
                }

                var dbRecordCourse = await _courseRepository.FindAsync(input.CourseId);
                if (dbRecordCourse == null)
                {
                    result.Code = ResultStatusCode.NotFound;
                    result.Messege = "Course not found";
                    result.Result = false;
                    return result;
                }
                var dbRecordRating = await _ratingRepository.GetQuery()
                    .Include(c => c.Course).Include(u => u.User)
                    .Where(x => x.UserId == user.Id && x.CourseId == input.CourseId)
                    .FirstOrDefaultAsync();
                if (dbRecordRating != null)
                    await _ratingRepository.DeleteAsync(dbRecordRating.Id);

                var rating = _mapper.Map<RatingInput, Rating>(input);
                rating.UserId = user.Id;
                await _ratingRepository.CreateAsync(rating);
                result.Code = ResultStatusCode.Ok;
                result.Messege = "Course rated";
                result.Result = true;
                return result;
            }
            catch
            {
                result.Code = ResultStatusCode.InternalServerError;
                result.Messege = "Excaption happen when rating course";
                result.Result = false;
                return result;
            }
        }

        public async Task<ResultService<bool>> DeleteRating(int id, User user)
        {
            var result = new ResultService<bool>();
            try
            {
                if (user == null)
                {
                    result.Code = ResultStatusCode.Unauthorized;
                    result.Messege = "You are Unauthorized";
                    result.Result = false;
                    return result;
                }
                var rating = await _ratingRepository.FindAsync(id);
                if (rating == null)
                {
                    result.Code = ResultStatusCode.BadRequest;
                    result.Messege = @"You can't remove rating not exist";
                    result.Result = false;
                    result.ErrorField = nameof(id);
                    return result;
                }
                await _ratingRepository.DeleteAsync(id);
                result.Code = ResultStatusCode.Ok;
                result.Messege = "Course rate deleted";
                result.Result = true;
                return result;
            }
            catch
            {
                result.Code = ResultStatusCode.InternalServerError;
                result.Messege = "Exception happen when rating course";
                result.Result = true;
                return result;
            }
        }

        public ResultService<Dictionary<string, int>> GetRatingsData()
        {
            var ratings = Enum.GetValues<RatingStar>();
            Dictionary<string, int> result = new();
            foreach (var rating in ratings)
            {
                result.Add(rating.ToString(), (int)rating);
            }
            return new ResultService<Dictionary<string, int>>()
            {
                Code = ResultStatusCode.Ok,
                Messege = "Success",
                Result = result,
            };
        }

    }
    public interface IRatingService
    {
        public ResultService<Dictionary<string, int>> GetRatingsData();
        public Task<ResultService<bool>> RatingCourseAsync(RatingInput input, User user);
        public Task<ResultService<RatingOutput>> GetRatingForCourseAsync(int id);
        public Task<ResultService<bool>> DeleteRating(int id, User user);
    }
}