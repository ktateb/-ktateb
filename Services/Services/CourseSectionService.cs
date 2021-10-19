using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities.Courses;
using DAL.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Services
{
    public class CourseSectionService : ICourseSectionService
    {
        private readonly IGenericRepository<CourseSection> _ICourseSectionRepository;
        private readonly IGenericRepository<CourseVedio> _ICourseVedioRepository;
        public CourseSectionService(IGenericRepository<CourseSection> ICourseSectionRepository, IGenericRepository<CourseVedio> ICourseVedioRepository)
        {
            _ICourseSectionRepository = ICourseSectionRepository;
            _ICourseVedioRepository = ICourseVedioRepository;
        }
        public async Task<CourseSection> GetSectionAsync(int Id) =>
            await _ICourseSectionRepository.GetQuery().Where(c => c.Id == Id).FirstOrDefaultAsync();
        public async Task<List<CourseVedio>> GetVediosInfoAsync(int SectionId) =>
            await _ICourseVedioRepository.GetQuery().Where(c => c.SectionId == SectionId).OrderBy(c => c.VedioTitle).ToListAsync();
        public async Task<bool> CreateSectionInfoAsync(CourseSection Section) =>
            await _ICourseSectionRepository.CreateAsync(Section);
        public async Task<bool> DeleteSectionAsync(int Id) =>
            await _ICourseSectionRepository.DeleteAsync(Id);
        public async Task<bool> UpdateSectionInfoAsync(CourseSection Section) =>
            await _ICourseSectionRepository.UpdateAsync(Section);

        public async Task<int> GetTeacerIdAsync(int SectionId) =>
            await _ICourseSectionRepository.GetQuery().Where(c => c.Id==SectionId).Select(c => c.Course.TeacherId).FirstOrDefaultAsync();
        public async Task<int> GetCourseIdAsync(int SectionId) =>
            await _ICourseSectionRepository.GetQuery().Where(c => c.Id==SectionId).Select(c => c.CourseId).FirstOrDefaultAsync();

    }
    public interface ICourseSectionService
    {
        public Task<CourseSection> GetSectionAsync(int Id);
        public Task<List<CourseVedio>> GetVediosInfoAsync(int SectionId);
        public Task<bool> CreateSectionInfoAsync(CourseSection Section);
        public  Task<int> GetCourseIdAsync(int SectionId);
        public Task<int> GetTeacerIdAsync(int SectionId);
        public Task<bool> UpdateSectionInfoAsync(CourseSection Section);
        public Task<bool> DeleteSectionAsync(int Id);
    }
}