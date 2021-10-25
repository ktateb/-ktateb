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
using Model.Report.Comment.Inputs;
using Model.Report.Course.Inputs;
using Model.Report.Message.Inputs;
using Model.Report.User.Inputs;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<ReportComment> _reportCommentRepository;
        private readonly IGenericRepository<ReportUser> _reportUserRepository;
        private readonly IGenericRepository<ReportMessage> _reportMessageRepository;
        private readonly IGenericRepository<ReportCourse> _reportCourseRepository;
        private readonly IGenericRepository<ReportSubComment> _reportSubCommentRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IGenericRepository<SubComment> _subcommentRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IIdentityRepository _identityRepository;
        private readonly IMapper _mapper;


        public ReportService(IGenericRepository<ReportComment> reportCommentRepository, IGenericRepository<ReportUser> reportUserRepository,
                    IGenericRepository<ReportMessage> reportMessageRepository, IGenericRepository<ReportCourse> reportCourseRepository,
                    IGenericRepository<ReportSubComment> reportSubCommentRepository, IGenericRepository<Message> messageRepository,
                    IGenericRepository<Comment> commentRepository, IGenericRepository<Course> courseRepository, IIdentityRepository identityRepository,
                    IGenericRepository<SubComment> subcommentRepository, IMapper mapper)
        {
            _reportCommentRepository = reportCommentRepository;
            _reportUserRepository = reportUserRepository;
            _reportMessageRepository = reportMessageRepository;
            _reportCourseRepository = reportCourseRepository;
            _reportSubCommentRepository = reportSubCommentRepository;
            _messageRepository = messageRepository;
            _commentRepository = commentRepository;
            _courseRepository = courseRepository;
            _subcommentRepository = subcommentRepository;
            _identityRepository = identityRepository;
            _mapper = mapper;
        }


    public async Task<ResultService<bool>> ReportComment(ReportCommentInput input, User user)
    {
        var result = new ResultService<bool>();
        if (user == null)
        {
            result.Messege = "You are unauthorized";
            result.Code = ResultStatusCode.Unauthorized;
            result.Result = false;
            return result;
        }
        var dbRecordComment = await _commentRepository.FindAsync(input.CommentId);
        if (dbRecordComment == null)
        {
            result.Messege = "Comment not found";
            result.ErrorField = nameof(input.CommentId);
            result.Code = ResultStatusCode.NotFound;
            result.Result = false;
            return result;
        }
        try
        {
            var report = _mapper.Map<ReportCommentInput, ReportComment>(input);
            report.UserId = user.Id;
            report.DateReport = DateTime.UtcNow;
            await _reportCommentRepository.CreateAsync(report);
            result.Messege = "Comment reported";
            result.Result = true;
            return result;
        }
        catch
        {
            result.Messege = "Exception happen when report comment";
            result.Code = ResultStatusCode.InternalServerError;
            result.Result = false;
            return result;
        }
    }

    public async Task<ResultService<bool>> ReportCourse(ReportCourseInput input, User user)
    {

        var result = new ResultService<bool>();
        if (user == null)
        {
            result.Messege = "You are unauthorized";
            result.Code = ResultStatusCode.Unauthorized;
            result.Result = false;
            return result;
        }
        var dbRecordCourse = await _courseRepository.FindAsync(input.CourseId);
        if (dbRecordCourse == null)
        {
            result.Messege = "Course not found";
            result.ErrorField = nameof(input.CourseId);
            result.Code = ResultStatusCode.NotFound;
            result.Result = false;
            return result;
        }
        try
        {
            var report = _mapper.Map<ReportCourseInput, ReportCourse>(input);
            report.DateReport = DateTime.UtcNow;
            report.UserId = user.Id;
            await _reportCourseRepository.CreateAsync(report);
            result.Messege = "Course reported";
            result.Result = true;
            return result;
        }
        catch
        {
            result.Messege = "Exception happen when report course";
            result.Code = ResultStatusCode.InternalServerError;
            result.Result = false;
            return result;
        }
    }

    public async Task<ResultService<bool>> ReportMessage(ReportMessageInput input, User user)
    {
        var result = new ResultService<bool>();

        if (user == null)
        {
            result.Messege = "You are unauthorized";
            result.Code = ResultStatusCode.Unauthorized;
            result.Result = false;
            return result;
        }

        var dbRecordMessage = await _messageRepository.FindAsync(input.MessageId);
        if (dbRecordMessage == null)
        {
            result.Messege = "Message not found";
            result.ErrorField = nameof(input.MessageId);
            result.Code = ResultStatusCode.NotFound;
            result.Result = false;
            return result;
        }
        if (dbRecordMessage.SenderId == user.Id)
        {
            result.Messege = "You can't report your message Lol";
            result.ErrorField = nameof(input.MessageId);
            result.Code = ResultStatusCode.BadRequest;
            result.Result = false;
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
            return result;
        }
        catch
        {
            result.Messege = "Exception happen when report message";
            result.Code = ResultStatusCode.InternalServerError;
            result.Result = false;
            return result;
        }
    }

    public async Task<ResultService<bool>> ReportUser(ReportUserInput input, User user)
    {
        var result = new ResultService<bool>();

        if (user == null)
        {
            result.Messege = "You are unauthorized";
            result.Code = ResultStatusCode.Unauthorized;
            result.Result = false;
            return result;
        }
        if (user.Id == input.UserReciveReportId)
        {
            result.Messege = @"You can't report yourself Lol";
            result.Code = ResultStatusCode.BadRequest;
            result.ErrorField = nameof(input.UserReciveReportId);
            result.Result = false;
            return result;
        }
        var dbRecordUser = await _identityRepository.GetUserByIdAsync(input.UserReciveReportId);
        if (dbRecordUser == null)
        {
            result.Messege = "User not found";
            result.Code = ResultStatusCode.NotFound;
            result.ErrorField = nameof(input.UserReciveReportId);
            result.Result = false;
            return result;
        }
        try
        {
            var report = _mapper.Map<ReportUserInput, ReportUser>(input);
            report.DateReport = DateTime.UtcNow;
            report.UserSendReportId = user.Id;
            await _reportUserRepository.CreateAsync(report);
            result.Messege = "User reported";
            result.Result = true;
            return result;
        }
        catch
        {
            result.Messege = "Exception happen when report user";
            result.Code = ResultStatusCode.InternalServerError;
            result.Result = false;
            return result;
        }
    }

    public async Task<ResultService<bool>> ReportSubComment(ReportCommentInput input, User user)
    {
        var result = new ResultService<bool>();
        if (user == null)
        {
            result.Messege = "You are unauthorized";
            result.Code = ResultStatusCode.Unauthorized;
            result.Result = false;
            return result;
        }
        var dbRecordSubcommentRepository = await _subcommentRepository.FindAsync(input.CommentId);
        if (dbRecordSubcommentRepository == null)
        {
            result.Messege = "Sub comment not found";
            result.Code = ResultStatusCode.NotFound;
            result.Result = false;
            return result;
        }
        try
        {
            var report = _mapper.Map<ReportCommentInput, ReportSubComment>(input);
            report.UserId = user.Id;
            report.DateReport = DateTime.UtcNow;
            await _reportSubCommentRepository.CreateAsync(report);
            result.Messege = "Sub comment reported";
            result.Result = true;
            return result;
        }
        catch
        {
            result.Messege = "Exception happen when report sub comment";
            result.Code = ResultStatusCode.InternalServerError;
            result.Result = false;
            return result;
        }
    }
}
public interface IReportService
{
    public Task<ResultService<bool>> ReportMessage(ReportMessageInput input, User user);
    public Task<ResultService<bool>> ReportComment(ReportCommentInput input, User user);
    public Task<ResultService<bool>> ReportCourse(ReportCourseInput input, User user);
    public Task<ResultService<bool>> ReportUser(ReportUserInput input, User user);
    public Task<ResultService<bool>> ReportSubComment(ReportCommentInput input, User user);
}
}