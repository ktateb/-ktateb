using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities.Courses;
using DAL.Entities.Teachers;
using DAL.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Common.Services;
using Model.Teacher.Outputs;
using AutoMapper;
using Model.Teacher.Inputs;

namespace Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IGenericRepository<Teacher> _ITeacherRepository;
        private readonly IGenericRepository<Course> _iCourseRepository;
        private readonly IMapper _mapper;
        public TeacherService(IMapper mapper, IGenericRepository<Teacher> ITeacherRepository, IGenericRepository<Course> iCourseRepository)
        {
            _mapper = mapper;
            _ITeacherRepository = ITeacherRepository;
            _iCourseRepository = iCourseRepository;
        }
        public async Task<ResultService<TeacherOutput>> GetTeacherInfoAsync(string UserName)
        {
            ResultService<TeacherOutput> result = new ();
            var teacher = await _ITeacherRepository.GetQuery().Where(t => t.User.NormalizedUserName.Equals(UserName.ToUpper())).FirstOrDefaultAsync();
            if (teacher == null)
                result.SetCode(ResultStatusCode.NotFound).SetMessege("no Teacher with " + UserName + " UserName found");
            else
                result.SetResult(_mapper.Map<Teacher, TeacherOutput>(teacher));
            return result;
        }
        public async Task<List<Course>> GetTeacherCoursesAsync(string username) =>
            await _iCourseRepository.GetQuery().Where(c => c.Teacher.User.NormalizedUserName.Equals(username.ToUpper())).ToListAsync();

        public async Task<int> GetTeacherIdOrDefaultAsync(string UserId) =>
          await _ITeacherRepository.GetQuery().Where(t => t.UserId.Equals(UserId)).Select(t => t.Id).FirstOrDefaultAsync();
        public async Task<ResultService<TeacherOutput>> UpdateTeacherInfoAsync(TeacherUpdateInput teacher, string userId)
        {
            try
            {
                ResultService<TeacherOutput> result = new();
                var techerid = await GetTeacherIdOrDefaultAsync(userId);
                if (default == techerid)
                    return result.SetCode(ResultStatusCode.NotFound).SetMessege("Not Found 404");
                var teacherToUpdate = _mapper.Map<TeacherUpdateInput, Teacher>(teacher);
                teacherToUpdate.Id = techerid;
                teacherToUpdate.UserId = userId;
                if (await _ITeacherRepository.UpdateAsync(teacherToUpdate))
                    return result.SetResult(_mapper.Map<Teacher, TeacherOutput>(teacherToUpdate)).SetMessege("Done");
                return result.SetMessege("Data not updated").SetCode(ResultStatusCode.BadRequest);
            }
            catch
            {
                return ResultService<TeacherOutput>.GetErrorResult();
            }
        }
    }
    public interface ITeacherService
    {
        public Task<int> GetTeacherIdOrDefaultAsync(string UserId);
        public Task<ResultService<TeacherOutput>> GetTeacherInfoAsync(string UserName);
        public Task<List<Course>> GetTeacherCoursesAsync(string username);
        public Task<ResultService<TeacherOutput>> UpdateTeacherInfoAsync(TeacherUpdateInput teacher, string userId);
    }
}