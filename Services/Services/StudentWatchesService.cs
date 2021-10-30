using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Services;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using DAL.Entities.StudentCourses;
using DAL.Entities.StudentWatches;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Helper;

namespace Services.Services
{
    public class StudentWatchesService
    {
        private readonly IGenericRepository<CourseVedio> _iCourseVedioRepository;
        private readonly IGenericRepository<StudentWatchedVedio> _iWatchesRepository;
        private readonly IGenericRepository<StudentCourse> _iStudentCourseRepository;
        public StudentWatchesService(IGenericRepository<StudentCourse> iStudentCourseRepository, IGenericRepository<CourseVedio> ICourseVedioRepository, IGenericRepository<StudentWatchedVedio> iWatchesRepository)
        {
            _iStudentCourseRepository = iStudentCourseRepository;
            _iCourseVedioRepository = ICourseVedioRepository;
            _iWatchesRepository = iWatchesRepository;
        }
        public async Task<PagedList<StudentWatchedVedio>> GetWatchedListAsync(User user, Paging Params)
        {
            var Query = _iWatchesRepository.GetQuery().Where(i => i.UsertId.Equals(user.Id)).OrderByDescending(i => i.WatchedDate);
            return await PagedList<StudentWatchedVedio>.CreatePagingListAsync(Query, Params.PageNumber, Params.PageSize);
        }

        private async Task<bool> CanAdd(int VedioId, User User)
        {
            var courseId = await _iCourseVedioRepository.GetQuery().Where(s => s.Id == VedioId).Select(s => s.Section.CourseId).FirstOrDefaultAsync();
            return await _iStudentCourseRepository.GetQuery().Where(s => s.CourseId == courseId && s.UserId.Equals(User.Id)).AnyAsync();
        }

        public async Task<ResultService<bool>> AddToWatchedAsync(int VedioId, User User)
        {
            ResultService<bool> result = new();
            try
            {

                if (!await _iCourseVedioRepository.IsExist(VedioId))
                {
                    return result.SetCode(ResultStatusCode.NotFound)
                        .SetMessege("Vedio Not Found")
                        .SetResult(false)
                        .SetErrorField(nameof(VedioId));
                }

                var Watched = await Get(VedioId, User);
                if (Watched is null)
                {
                    Watched = new() { WatchedDate = DateTime.Now, VedioId = VedioId, UsertId = User.Id };
                    if (await CanAdd(VedioId,User)&&await _iWatchesRepository.CreateAsync(Watched))
                    {
                        return result.SetResult(true).SetMessege("vedio added");
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
                        .SetMessege("Vedio is in your Watched List")
                        .SetResult(false);
                }
            }
            catch { return ResultService<bool>.GetErrorResult().SetResult(false); }
        }
        public async Task<ResultService<bool>> RemoveFromWatchedAsync(int VedioId, User User)
        {
            ResultService<bool> result = new();
            try
            {
                if (!await _iCourseVedioRepository.IsExist(VedioId))
                {
                    return result.SetCode(ResultStatusCode.NotFound)
                        .SetMessege("Vedio Not Found")
                        .SetResult(false)
                        .SetErrorField(nameof(VedioId));
                }

                var Watched = await Get(VedioId, User);
                if (Watched is null)
                {
                    return result.SetCode(ResultStatusCode.BadRequest)
                        .SetMessege("Vedio is not in your Watched List")
                        .SetResult(false);
                }
                else
                {
                    if (await _iWatchesRepository.DeleteAsync(Watched.Id))
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
        private Task<StudentWatchedVedio> Get(int VedioId, User User) =>
            _iWatchesRepository.GetQuery().Where(s => s.VedioId == VedioId && s.UsertId.Equals(User.Id)).FirstOrDefaultAsync();


    }
    public interface IStudentWatchesService
    {
        public Task<PagedList<StudentWatchedVedio>> GetWatchedListAsync(User user, Paging Params);

        public Task<ResultService<bool>> AddToWatchedAsync(int VedioId, User user);

        public Task<ResultService<bool>> RemoveFromWatchedAsync(int VedioId, User user);
    }
}