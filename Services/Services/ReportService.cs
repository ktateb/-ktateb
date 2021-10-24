using System;
using System.Threading.Tasks;
using AutoMapper;
using Common.Services;
using DAL.Entities.Comments;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using DAL.Entities.Messages;
using DAL.Entities.Reports;
using DAL.Repositories;
using Model.Report.Message.Inputs;
using Model.Report.Message.Outputs;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<ReportComment> _reportCommentRepository;
        private readonly IGenericRepository<ReportUser> _reportUserRepository;
        private readonly IGenericRepository<ReportMessage> _reportMessageRepository;
        private readonly IGenericRepository<ReportCourse> _reportCourseRepository;
        private readonly IGenericRepository<ReportSubComment> _reportSubCommentRepository;
        public IGenericRepository<Message> _messageRepository;
        public IMapper _mapper;

        public ReportService(IGenericRepository<ReportComment> reportCommentRepository, IGenericRepository<ReportUser> reportUserRepository,
            IGenericRepository<ReportMessage> reportMessageRepository, IGenericRepository<ReportCourse> reportCourseRepository,
            IGenericRepository<ReportSubComment> reportSubCommentRepository, IGenericRepository<Message> messageRepository,
            IMapper mapper)
        {
            _reportCommentRepository = reportCommentRepository;
            _reportUserRepository = reportUserRepository;
            _reportMessageRepository = reportMessageRepository;
            _reportCourseRepository = reportCourseRepository;
            _reportSubCommentRepository = reportSubCommentRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        public async Task<bool> ReportComment(ReportComment report) =>
            await _reportCommentRepository.CreateAsync(report);

        public async Task<bool> ReportCourse(ReportCourse report) =>
            await _reportCourseRepository.CreateAsync(report);

        public async Task<ResultService<bool>> ReportMessage(ReportMessageInput input, User user)
        {
            var result = new ResultService<bool>();

            if (user == null)
            {
                result.ErrorHappen = true;
                result.Messege = "You are unauthorized";
                result.Code = ResultStatusCode.Unauthorized;
                return result;
            }

            var dbRecordMessage = await _messageRepository.FindAsync(input.MessageId);
            if (dbRecordMessage == null)
            {
                result.ErrorHappen = true;
                result.Messege = "Message not found";
                result.ErrorField = "MessageId";
                result.Code = ResultStatusCode.NotFound;
                return result;
            }
            if (dbRecordMessage.SenderId == user.Id)
            {
                result.ErrorHappen = true;
                result.Messege = "You can't report you message Lol";
                result.ErrorField = "MessageId";
                result.Code = ResultStatusCode.BadRequist;
                return result;
            }

            try
            {
                var report = _mapper.Map<ReportMessageInput, ReportMessage>(input);
                report.DateReport = DateTime.UtcNow;
                report.UserId = user.Id;
                await _reportMessageRepository.CreateAsync(report);
                result.Messege = "Message reported";
                result.Result = true;
                result.Code = ResultStatusCode.Ok;
                return result;
            }
            catch
            {
                result.ErrorHappen = true;
                result.Messege = "Exception Happen when Report Message";
                result.Code = ResultStatusCode.InternalServerError;
                return result;
            }
        }

        public async Task<bool> ReportUser(ReportUser report) =>
            await _reportUserRepository.CreateAsync(report);

        public async Task<bool> ReportSubComment(ReportSubComment report) =>
            await _reportSubCommentRepository.CreateAsync(report);
    }
    public interface IReportService
    {
        public Task<ResultService<bool>> ReportMessage(ReportMessageInput input, User user);
        public Task<bool> ReportComment(ReportComment report);
        public Task<bool> ReportCourse(ReportCourse report);
        public Task<bool> ReportUser(ReportUser report);
        public Task<bool> ReportSubComment(ReportSubComment report);
    }
}