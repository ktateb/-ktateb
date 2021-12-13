using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities.Courses;
using DAL.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Common.Services;

namespace Services
{
    public class CourseSectionService : ICourseSectionService
    {

        private readonly IGenericRepository<CourseVedio> _iCourseVedio;
        private readonly IGenericRepository<CourseSection> _ICourseSectionRepository;
        private readonly IGenericRepository<CourseVedio> _ICourseVedioRepository;
        public CourseSectionService(IGenericRepository<CourseVedio> iCourseVedio,IGenericRepository<CourseSection> ICourseSectionRepository, IGenericRepository<CourseVedio> ICourseVedioRepository)
        {
            _iCourseVedio = iCourseVedio;
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
            await _ICourseSectionRepository.GetQuery().Where(c => c.Id == SectionId).Select(c => c.Course.TeacherId).FirstOrDefaultAsync();
        public async Task<int> GetCourseIdAsync(int SectionId) =>
            await _ICourseSectionRepository.GetQuery().Where(c => c.Id == SectionId).Select(c => c.CourseId).FirstOrDefaultAsync();

        public async Task<ResultService<int>> GetTotalTimeInSeconds(int SectionId)
        {
            try
            {
                ResultService<int> result = new();
                result.Result = await _iCourseVedio.GetQuery().Where(c => c.SectionId == SectionId).SumAsync(s => s.TimeInSeconds);
                return result;
            }
            catch { return ResultService<int>.GetErrorResult().SetResult(default); }
        }
    }
    public interface ICourseSectionService
    {
        public Task<ResultService<int>> GetTotalTimeInSeconds(int SectionId);
        public Task<CourseSection> GetSectionAsync(int Id);
        public Task<List<CourseVedio>> GetVediosInfoAsync(int SectionId);
        public Task<bool> CreateSectionInfoAsync(CourseSection Section);
        public Task<int> GetCourseIdAsync(int SectionId);
        public Task<int> GetTeacerIdAsync(int SectionId);
        public Task<bool> UpdateSectionInfoAsync(CourseSection Section);
        public Task<bool> DeleteSectionAsync(int Id);
    }
}