using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities.StudentFavoriteCourses;
using DAL.Entities.Courses;
using DAL.Repositories;
using AutoMapper;
using Common.Services;
using Model.Helper;
using DAL.Entities.Identity;
using Model.Course.Outputs;
using Microsoft.EntityFrameworkCore;

namespace Services.Services
{
    public class FavoriteCoursesService : IFavoriteCoursesService
    {
        private readonly IGenericRepository<Course> _iCourseRepository;
        private readonly IGenericRepository<StudentFavoriteCourse> _iFavoriteRepository;

        public FavoriteCoursesService(IGenericRepository<Course> ICourseRepository, IGenericRepository<StudentFavoriteCourse> iFavoriteCourseRepository)
        {
            _iCourseRepository = ICourseRepository;
            _iFavoriteRepository = iFavoriteCourseRepository;
        }

        public async Task<PagedList<StudentFavoriteCourse>> GetFavoriteListAsync(User user, Paging Params)
        {
            var Query = _iFavoriteRepository.GetQuery().Where(i => i.UserId.Equals(user.Id)).OrderByDescending(i => i.AddedDate);
            return await PagedList<StudentFavoriteCourse>.CreatePagingListAsync(Query, Params.PageNumber, Params.PageSize);
        }

        public async Task<ResultService<bool>> AddToFavoriteAsync(int CourseId, User User)
        {
            ResultService<bool> result = new();
            try
            {

                if (!await _iCourseRepository.IsExist(CourseId))
                {
                    return result.SetCode(ResultStatusCode.NotFound)
                        .SetMessege("Course Not Found")
                        .SetResult(false)
                        .SetErrorField(nameof(CourseId));
                }

                var Favorite = await Get(CourseId, User);
                if (Favorite is null)
                {
                    Favorite = new() { AddedDate = DateTime.Now, CourseId = CourseId, UserId = User.Id };
                    if (await _iFavoriteRepository.CreateAsync(Favorite))
                    {
                        return result.SetResult(true);
                    }
                    else
                    {
                        return result.SetResult(false)
                            .SetCode(ResultStatusCode.BadRequest)
                            .SetMessege("Not Added");
                    }

                }
                else
                {
                    return result.SetCode(ResultStatusCode.BadRequest)
                        .SetMessege("Course is in your Favorite List")
                        .SetResult(false);
                }
            }
            catch { return ResultService<bool>.GetErrorResult().SetResult(false); }
        }
        public async Task<ResultService<bool>> RemoveFromFavoriteAsync(int CourseId, User User)
        {
            ResultService<bool> result = new();
            try
            {
                if (!await _iCourseRepository.IsExist(CourseId))
                {
                    return result.SetCode(ResultStatusCode.NotFound)
                        .SetMessege("Course Not Found")
                        .SetResult(false)
                        .SetErrorField(nameof(CourseId));
                }

                var Favorite = await Get(CourseId, User);
                if (Favorite is null)
                {
                    return result.SetCode(ResultStatusCode.BadRequest)
                        .SetMessege("Course is not in your Favorite List")
                        .SetResult(false);
                }
                else
                {
                    if (await _iFavoriteRepository.DeleteAsync(Favorite.Id))
                    {
                        return result.SetResult(true);
                    }
                    else
                    {
                        return result.SetResult(false)
                            .SetCode(ResultStatusCode.BadRequest)
                            .SetMessege("Not Removed");
                    }
                }
            }
            catch { return ResultService<bool>.GetErrorResult().SetResult(false); }
        }
        public async Task<ResultService<bool>> isFavoriteByMe(int CourseId, string Userid) =>
            new ResultService<bool>().SetResult(await _iFavoriteRepository.GetQuery().Where(s => s.CourseId == CourseId && s.UserId.Equals(Userid)).AnyAsync());
        private Task<StudentFavoriteCourse> Get(int CourseId, User User) =>
            _iFavoriteRepository.GetQuery().Where(s => s.CourseId == CourseId && s.UserId.Equals(User.Id)).FirstOrDefaultAsync();

    }
    public interface IFavoriteCoursesService
    {
        public Task<ResultService<bool>> isFavoriteByMe(int CourseId, string Userid);
        public Task<PagedList<StudentFavoriteCourse>> GetFavoriteListAsync(User user, Paging parms);

        public Task<ResultService<bool>> AddToFavoriteAsync(int CourseId, User user);

        public Task<ResultService<bool>> RemoveFromFavoriteAsync(int CourseId, User user);
    }
}