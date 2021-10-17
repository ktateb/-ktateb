using System.Threading.Tasks;
using DAL.Entities.Comments;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using DAL.Entities.Messages;
using DAL.Entities.Reports;
using DAL.Repositories;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<ReportComment> _reportCommentRepository;
        private readonly IGenericRepository<ReportUser> _reportUserRepository;
        private readonly IGenericRepository<ReportMessage> _reportMessageRepository;
        private readonly IGenericRepository<ReportCourse> _reportCourseRepository;

        public ReportService(IGenericRepository<ReportComment> reportCommentRepository
            , IGenericRepository<ReportUser> reportUserRepository, IGenericRepository<ReportMessage> reportMessageRepository,
             IGenericRepository<ReportCourse> reportCourseRepository)
        {
            _reportCommentRepository = reportCommentRepository;
            _reportUserRepository = reportUserRepository;
            _reportMessageRepository = reportMessageRepository;
            _reportCourseRepository = reportCourseRepository;
        }
        public async Task<bool> ReportComment(ReportComment report) =>
            await _reportCommentRepository.CreateAsync(report);

        public async Task<bool> ReportCourse(ReportCourse report) =>
            await _reportCourseRepository.CreateAsync(report);

        public async Task<bool> ReportMessage(ReportMessage report) =>
            await _reportMessageRepository.CreateAsync(report);

        public async Task<bool> ReportUser(ReportUser report) =>
            await _reportUserRepository.CreateAsync(report);
    }
    public interface IReportService
    {
        public Task<bool> ReportMessage(ReportMessage report);
        public Task<bool> ReportComment(ReportComment report);
        public Task<bool> ReportCourse(ReportCourse report);
        public Task<bool> ReportUser(ReportUser report);
    }
}