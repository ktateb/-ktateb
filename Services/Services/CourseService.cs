using DAL.Entities.Courses;
using DAL.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using DAL.Entities.StudentCourses;
using System;
using DAL.Entities.Comments;
using Model.Helper;
using AutoMapper;
using Common.Services;
using Model.Course.Outputs;
using Model.CourseSection.Outputs;
using Model.Course.Inputs;
using DAL.Entities.Identity;

namespace Services
{
    public class CourseService : ICourseService
    {
        private readonly IGenericRepository<Course> _iCourseRepository;
        private readonly IGenericRepository<CourseSection> _iCourseSectionRepository;
        private readonly IGenericRepository<StudentCourse> _iStudentCourseRepository;
        private readonly IGenericRepository<CoursePriceHistory> _iCoursePriceHistory;

        private readonly IGenericRepository<CourseVedio> _iCourseVedio;
        private readonly IGenericRepository<Comment> _iCommentRepository;
        private readonly IMapper _mapper;
        public CourseService(IGenericRepository<CourseVedio> iCourseVedio, IMapper mapper, IGenericRepository<CoursePriceHistory> iCoursePriceHistory, IGenericRepository<Comment> iCommentRepository, IGenericRepository<StudentCourse> iStudentCourseRepository, IGenericRepository<Course> iCourseRepository, IGenericRepository<CourseSection> iCourseSectionRepository)
        {
            _iCourseVedio = iCourseVedio;
            _mapper = mapper;
            _iCoursePriceHistory = iCoursePriceHistory;
            _iStudentCourseRepository = iStudentCourseRepository;
            _iCourseRepository = iCourseRepository;
            _iCourseSectionRepository = iCourseSectionRepository;
            _iCommentRepository = iCommentRepository;
        }

        public async Task<ResultService<CourseOutput>> GetCourseInfoAsync(int Id)
        {
            try
            {
                var Course = await _iCourseRepository.GetQuery().Where(c => c.Id == Id).Include(c => c.Category).Include(s => s.PriceHistory).Include(c => c.Teacher).ThenInclude(t => t.User).FirstOrDefaultAsync();
                var Result = new ResultService<CourseOutput>();
                if (Course == null)
                    return Result.SetCode(ResultStatusCode.NotFound).SetMessege("Course Not Found").SetErrorField(nameof(Id));
                var CourseOutput = _mapper.Map<Course, CourseOutput>(Course);
                return Result.SetCode(ResultStatusCode.Ok).SetResult(CourseOutput).SetMessege("Done");
            }
            catch
            {
                return ResultService<CourseOutput>.GetErrorResult();
            }
        }
        public async Task<ResultService<CourseOutput>> GetCourseInfoAsync(int Id, string userid)
        {
            var result = await GetCourseInfoAsync(Id);
            if (result.Code != ResultStatusCode.Ok)
                return result;
            try
            {
                result.Result.IsOwned = await _iStudentCourseRepository.GetQuery().Where(c => c.UserId == userid && c.CourseId == Id).AnyAsync();
                return result;
            }
            catch
            {
                return ResultService<CourseOutput>.GetErrorResult();
            }
        }
        public async Task<ResultService<List<CourseSectionOutput>>> GetCourseSectionAsync(int CourseId)
        {
            try
            {
                var Result = new ResultService<List<CourseSectionOutput>>();
                if (!(await IsExistAsync(CourseId)))
                    return Result.SetCode(ResultStatusCode.NotFound).SetErrorField(nameof(CourseId)).SetMessege("Course not found");
                var Section = await _iCourseSectionRepository.GetQuery().Where(c => c.CourseId == CourseId).OrderBy(c => c.SectionTitle).ToListAsync();
                return Result.SetMessege("Ok").SetResult(_mapper.Map<List<CourseSection>, List<CourseSectionOutput>>(Section));
            }
            catch
            {
                return ResultService<List<CourseSectionOutput>>.GetErrorResult();
            }
        }
        public async Task<ResultService<CourseOutput>> CreateCoursesAsync(CourseCreateInput Course, int techerId)
        {
            try
            {
                var Result = new ResultService<CourseOutput>();
                if (Course.Price < 0)
                    return Result.SetCode(ResultStatusCode.BadRequest).SetMessege("new price is less than zero").SetErrorField(nameof(Course.Price));
                Course CourseToCreate = _mapper.Map<CourseCreateInput, Course>(Course);
                CourseToCreate.TeacherId = techerId;
                CourseToCreate.CreatedDate = DateTime.Now;
                CourseToCreate.PriceHistory = new() { new() { CourseId = CourseToCreate.Id, Price = Course.Price, StartedApplyDate = DateTime.Now } };
                if (await _iCourseRepository.CreateAsync(CourseToCreate))
                {
                    var CourseOutput = _mapper.Map<Course, CourseOutput>(CourseToCreate);
                    return Result.SetCode(ResultStatusCode.Ok).SetResult(CourseOutput).SetMessege("Done");
                }
                else
                {
                    return Result.SetCode(ResultStatusCode.BadRequest).SetMessege("Course not added");
                }
            }
            catch
            {
                return ResultService<CourseOutput>.GetErrorResult();
            }
        }
        private async Task<ResultService<T>> NeedToUpdateOrDelete<T>(int CourseId, int AuthtecherId)
        {
            var Result = new ResultService<T>();
            var CoursetecherIdTask = GetTeacherIdOrDefultAsync(CourseId);
            var CoursetecherId = await CoursetecherIdTask;
            if (default == CoursetecherId)
            {
                return Result.SetCode(ResultStatusCode.NotFound).SetMessege("Course not found").SetErrorField(nameof(CourseId));
            }
            if (CoursetecherId != AuthtecherId)
            {
                return Result.SetCode(ResultStatusCode.NotFound).SetMessege("You are not the Owner of this course").SetErrorField(nameof(CourseId));
            }
            return Result.SetMessege("Done");
        }
        public async Task<ResultService<bool>> DeleteCourseAsync(int CourseId, int AuthtecherId)
        {
            try
            {


                var HasStudentTask = HasStudentAsync(CourseId);
                var Result = await NeedToUpdateOrDelete<bool>(CourseId, AuthtecherId);
                if (Result.Code != ResultStatusCode.Ok)
                {
                    return Result.SetResult(false);
                }
                if (await HasStudentTask)
                {
                    return Result.SetCode(ResultStatusCode.BadRequest).SetMessege("this course Has Students").SetResult(false);
                }
                if (await _iCourseRepository.DeleteAsync(CourseId))
                    return Result.SetResult(true);
                else
                    return Result.SetCode(ResultStatusCode.BadRequest).SetMessege("not deleted").SetResult(false);
            }
            catch
            {
                return ResultService<bool>.GetErrorResult().SetResult(false);
            }

        }
        public async Task<ResultService<bool>> UpdateCourseInfoAsync(CourseUpdateInput CourseInput, int AuthTecherId)
        {
            try
            {
                var Result = new ResultService<bool>();
                var CoursetecherIdTask = GetTeacherIdOrDefultAsync(CourseInput.Id);
                var createdateTask = GetCourseCreatedDateCourseAsync(CourseInput.Id);
                var CoursetecherId = await CoursetecherIdTask;
                if (CoursetecherId == default)
                {
                    return Result.SetResult(false).SetCode(ResultStatusCode.NotFound).SetMessege("Course not found").SetErrorField(nameof(CourseInput.Id));
                }
                if (CoursetecherId != AuthTecherId)
                {
                    return Result.SetResult(false).SetCode(ResultStatusCode.Unauthorized).SetMessege("you are not the owner of this course").SetErrorField(nameof(CourseInput.Id));
                }
                Course course = _mapper.Map<CourseUpdateInput, Course>(CourseInput);
                course.TeacherId = CoursetecherId;
                course.CreatedDate = await createdateTask;
                if (await _iCourseRepository.UpdateAsync(course))
                    return Result.SetResult(true).SetMessege("Done");
                else
                    return Result.SetResult(false).SetMessege("not updated").SetCode(ResultStatusCode.BadRequest);
            }
            catch
            {
                return ResultService<bool>.GetErrorResult().SetResult(false);
            }
        }
        public async Task<ResultService<bool>> CreateNewPriceHistory(int CourseId, double newprice, int AuthTecherId)
        {
            try
            {
                var Result = await NeedToUpdateOrDelete<bool>(CourseId, AuthTecherId);
                if (newprice < 0)
                {
                    return Result.SetResult(false).SetCode(ResultStatusCode.BadRequest).SetMessege("new price is less than zero").SetErrorField(nameof(newprice));
                }
                if (Result.Code != ResultStatusCode.Ok)
                {
                    return Result.SetResult(false);
                }
                var oldprice = await GetPrice(CourseId);
                if (oldprice == newprice)
                {
                    return Result.SetResult(false).SetCode(ResultStatusCode.BadRequest).SetMessege("it is the same as old price").SetErrorField(nameof(newprice));
                }
                var NewPriceHistory = new CoursePriceHistory() { CourseId = CourseId, Price = newprice, StartedApplyDate = DateTime.Now };
                if (await _iCoursePriceHistory.CreateAsync(NewPriceHistory))
                {
                    return Result.SetResult(true);
                }
                else
                {
                    return Result.SetResult(false).SetCode(ResultStatusCode.BadRequest).SetMessege("new price not added");
                }
            }
            catch
            {
                return ResultService<bool>.GetErrorResult().SetResult(false);
            }
        }
        public async Task<ResultService<List<PriceHistoryOutput>>> GetPriceHistoryAsync(int CourseId)
        {
            try
            {
                var Result = new ResultService<List<PriceHistoryOutput>>();
                if (!(await IsExistAsync(CourseId)))
                    return Result.SetCode(ResultStatusCode.NotFound).SetErrorField(nameof(CourseId)).SetMessege("Course not found");
                var Course = await _iCourseRepository.GetQuery().Where(c => c.Id == CourseId).Include(c => c.PriceHistory).FirstOrDefaultAsync();
                var re = _mapper.Map<List<CoursePriceHistory>, List<PriceHistoryOutput>>(Course.PriceHistory.OrderBy(p => p.StartedApplyDate).ToList());
                return Result.SetResult(re);
            }
            catch
            {
                return ResultService<List<PriceHistoryOutput>>.GetErrorResult();
            }
        }
        public async Task<ResultService<bool>> FreeRegistAsync(int Id, string userId)
        {
            ResultService<bool> result = new();
            if (!(await _iStudentCourseRepository.GetQuery().Where(c => c.UserId == userId && c.CourseId == Id).AnyAsync()))
            {
                try
                {
                    StudentCourse newobg = new()
                    {
                        CourseId = Id,
                        UserId = userId,
                        RegistDate = DateTime.Now
                    };
                    if (await _iStudentCourseRepository.CreateAsync(newobg))
                        return result.SetResult(true);
                    else
                        return result.SetCode(ResultStatusCode.BadRequest).SetResult(false).SetMessege("Not Registed ,please try agin");
                }
                catch
                {
                    return ResultService<bool>.GetErrorResult().SetResult(false);
                }
            }
            else
                return result.SetCode(ResultStatusCode.BadRequest).SetResult(false).SetMessege("You are Owne have this course");

        }
        private async Task<bool> IsExistAsync(int Id) =>
          await _iCourseRepository.GetQuery().Where(c => c.Id == Id).AnyAsync();

        public async Task<int> GetTeacherIdOrDefultAsync(int CourseId) =>
           await _iCourseRepository.GetQuery().Where(c => c.Id == CourseId).Select(c => c.TeacherId).FirstOrDefaultAsync();

        private async Task<DateTime> GetCourseCreatedDateCourseAsync(int Id) =>
            await _iCourseRepository.GetQuery().Where(c => c.Id == Id).Select(c => c.CreatedDate).FirstOrDefaultAsync();


        public async Task<PagedList<Comment>> GetCommentsAsync(int CourseId, Paging Params)
        {
            var q = _iCommentRepository.GetQuery().Where(c => c.CourseId == CourseId).Include(s => s.User).OrderByDescending(c => c.DateComment);
            return await PagedList<Comment>.CreatePagingListAsync(q, Params.PageNumber, Params.PageSize);
        }
        public async Task<bool> HasStudentAsync(int Id) =>
            await _iStudentCourseRepository.GetQuery().Where(c => c.CourseId == Id).AnyAsync();
        private async Task<double> GetPrice(int Courseid) =>
             await _iCoursePriceHistory.GetQuery().Where(s => s.CourseId == Courseid && s.StartedApplyDate < DateTime.Now).OrderByDescending(s => s.StartedApplyDate).Select(s => s.Price).FirstOrDefaultAsync();

        public async Task<ResultService<int>> GetTotalTimeInSeconds(int CourseId)
        {
            try
            {

                ResultService<int> result = new();
                result.Result = await _iCourseVedio.GetQuery().Include(c => c.Section).Where(c => c.Section.CourseId == CourseId).SumAsync(s => s.TimeInSeconds);
                return result;
            }
            catch { return ResultService<int>.GetErrorResult().SetResult(default);}
        }
    }
    public interface ICourseService
    {
        public Task<ResultService<int>> GetTotalTimeInSeconds(int CourseId);
        public Task<PagedList<Comment>> GetCommentsAsync(int CourseId, Paging Params);
        public Task<int> GetTeacherIdOrDefultAsync(int CourseId);
        public Task<ResultService<CourseOutput>> GetCourseInfoAsync(int Id);
        public Task<ResultService<bool>> FreeRegistAsync(int Id, string userId);
        public Task<ResultService<CourseOutput>> GetCourseInfoAsync(int Id, string userid);
        public Task<ResultService<bool>> CreateNewPriceHistory(int CourseId, double newprice, int AuthTecherId);
        public Task<ResultService<List<PriceHistoryOutput>>> GetPriceHistoryAsync(int CourseId);
        public Task<ResultService<List<CourseSectionOutput>>> GetCourseSectionAsync(int CourseId);
        public Task<ResultService<CourseOutput>> CreateCoursesAsync(CourseCreateInput Course, int techerId);
        public Task<ResultService<bool>> UpdateCourseInfoAsync(CourseUpdateInput CourseInput, int AuthTecherId);
        public Task<ResultService<bool>> DeleteCourseAsync(int Id, int AuthTecherId);
        public Task<bool> HasStudentAsync(int Id);
    }
}