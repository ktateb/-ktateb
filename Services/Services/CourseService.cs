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

namespace Services
{
    public class CourseService : ICourseService
    {
        private readonly IGenericRepository<Course> _iCourseRepository;
        private readonly IGenericRepository<CourseSection> _iCourseSectionRepository;
        private readonly IGenericRepository<StudentCourse> _iStudentCourseRepository;

        private readonly IGenericRepository<Comment> _iCommentRepository;
        public CourseService(IGenericRepository<Comment> iCommentRepository,IGenericRepository<StudentCourse> iStudentCourseRepository, IGenericRepository<Course> iCourseRepository, IGenericRepository<CourseSection> iCourseSectionRepository)
        {
            _iStudentCourseRepository = iStudentCourseRepository;
            _iCourseRepository = iCourseRepository;
            _iCourseSectionRepository = iCourseSectionRepository;
            _iCommentRepository=iCommentRepository;
        }

        public async Task<Course> GetCourseInfoAsync(int Id) =>
           await _iCourseRepository.GetQuery().Where(c => c.Id == Id).Include(c => c.Category).Include(c => c.Teacher).ThenInclude(t => t.User).FirstOrDefaultAsync();
        public async Task<List<CourseSection>> GetCourseSectionAsync(int CourseId) =>
            await _iCourseSectionRepository.GetQuery().Where(c => c.CourseId == CourseId).OrderBy(c => c.SectionTitle).ToListAsync();
        public async Task<bool> CreateCoursesAsync(Course Course) =>
           await _iCourseRepository.CreateAsync(Course);
        public async Task<bool> DeleteCourseAsync(int Id) =>
            await _iCourseRepository.DeleteAsync(Id);
        public async Task<bool> HasStudentAsync(int Id) =>
            await _iStudentCourseRepository.GetQuery().Where(c => c.CourseId == Id).AnyAsync();
        public async Task<bool> UpdateCourseInfoAsync(Course Course) =>
           await _iCourseRepository.UpdateAsync(Course);

        public async Task<bool> IsExistAsync(int Id) =>
          await _iCourseRepository.GetQuery().Where(c => c.Id == Id).AnyAsync();

        public async Task<int> GetTeacherIdOrDefultAsync(int CourseId) =>
           await _iCourseRepository.GetQuery().Where(c => c.Id == CourseId).Select(c => c.TeacherId).FirstOrDefaultAsync();

        public async Task<DateTime> GetCourseCreatedDateCourseAsync(int Id) =>
            await _iCourseRepository.GetQuery().Select(c => c.CreatedDate).FirstOrDefaultAsync();
        public async Task<List<Comment>> GetCommentsAsync(int CourseId) =>
            await _iCourseRepository.GetQuery().Where(s => s.Id == CourseId).Include(c => c.Comments).ThenInclude(s => s.User).Select(c => c.Comments).FirstOrDefaultAsync();
        public async Task<PagedList<Comment>> GetCommentsAsync(int CourseId, Paging Params)
        {
            var q =  _iCommentRepository.GetQuery().Where(c => c.CourseId == CourseId).Include(s => s.User).OrderByDescending(c => c.DateComment);
            return await PagedList<Comment>.CreatePagingListAsync(q, Params.PageNumber, Params.PageSize);
        }
    }
    public interface ICourseService
    {
        public Task<PagedList<Comment>> GetCommentsAsync(int CourseId, Paging Params);
        public Task<bool> IsExistAsync(int Id);
        public Task<DateTime> GetCourseCreatedDateCourseAsync(int Id);
        public Task<int> GetTeacherIdOrDefultAsync(int CourseId);
        public Task<Course> GetCourseInfoAsync(int Id);

        public Task<List<CourseSection>> GetCourseSectionAsync(int Course);

        public Task<bool> CreateCoursesAsync(Course Course);

        public Task<bool> UpdateCourseInfoAsync(Course Course);

        public Task<bool> DeleteCourseAsync(int Id);

        public Task<bool> HasStudentAsync(int Id);
    }
}