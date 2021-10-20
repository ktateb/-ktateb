using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities.Courses;
using DAL.Entities.Teachers;
using DAL.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IGenericRepository<Teacher> _ITeacherRepository;
        private readonly IGenericRepository<Course> _iCourseRepository;
        public TeacherService(IGenericRepository<Teacher> ITeacherRepository, IGenericRepository<Course> iCourseRepository)
        {
            
            _ITeacherRepository = ITeacherRepository;
            _iCourseRepository = iCourseRepository;
        }

        public async Task<Teacher> GetTeacherInfoAsync(int TeacherId) =>
            await _ITeacherRepository.GetQuery().Where(t => t.Id == TeacherId).FirstOrDefaultAsync();
      
        public async Task<Teacher> GetTeacherInfoAsync(string UserName) =>
            await _ITeacherRepository.GetQuery().Where(t => t.User.NormalizedUserName.Equals(UserName.ToUpper())).FirstOrDefaultAsync();
      
        public async Task<List<Course>> GetTeacherCoursesAsync(int TeacherId) =>
           await _iCourseRepository.GetQuery().Where(c => c.TeacherId == TeacherId).ToListAsync();
      
        public async Task<List<Course>> GetTeacherCoursesAsync(string username) =>
            await _iCourseRepository.GetQuery().Where(c => c.Teacher.User.NormalizedUserName.Equals(username.ToUpper())).ToListAsync();
      
        public async Task<int> GetTeacherIdOrDefaultAsync(string UserId) =>
          await _ITeacherRepository.GetQuery().Where(t => t.UserId.Equals(UserId)).Select(t => t.Id).FirstOrDefaultAsync();
      
        public async Task<string> GetUserIdAsync(int TeacherId) =>
            await _ITeacherRepository.GetQuery().Where(t => t.Id == TeacherId).Select(t => t.UserId).FirstOrDefaultAsync();
      
        public async Task<bool> UpdateTeacherInfoAsync(Teacher teacher) =>
            await _ITeacherRepository.UpdateAsync(teacher);
 
    }
    public interface ITeacherService
    { 
        public Task<string> GetUserIdAsync(int TeacherId);
        public Task<int> GetTeacherIdOrDefaultAsync(string UserId);
        public Task<Teacher> GetTeacherInfoAsync(int TeacherId);
        public Task<Teacher> GetTeacherInfoAsync(string UserName);
        public Task<List<Course>> GetTeacherCoursesAsync(int TeacherId);
        public Task<List<Course>> GetTeacherCoursesAsync(string username);
        public Task<bool> UpdateTeacherInfoAsync(Teacher teacher);
    }
}