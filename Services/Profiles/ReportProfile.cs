using AutoMapper;
using DAL.Entities.Reports;
using Model.Report.Comment.Inputs;
using Model.Report.Comment.Outputs;
using Model.Report.Course.Inputs;
using Model.Report.Course.Outputs;
using Model.Report.Message.Inputs;
using Model.Report.Message.Outputs;
using Model.Report.User.Inputs;
using Model.Report.User.Outputs;

namespace Services.Profiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<ReportCommentInput, ReportComment>();
            CreateMap<ReportComment, ReportCommentOutput>();
            CreateMap<ReportComment, ReportCommentForDashboard>()
                .ForMember(dest => dest.ReportText, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserSendReport.UserName))
                .ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.Comment.Text))
                .ForMember(dest => dest.DateSentComment, opt => opt.MapFrom(src => src.Comment.DateComment))
                .ForMember(dest => dest.UserNameSentComment, opt => opt.MapFrom(src => src.UserSendReport.UserName));

            CreateMap<ReportCommentInput, ReportSubComment>();
            CreateMap<ReportSubComment, ReportCommentOutput>();
            CreateMap<ReportSubComment, ReportCommentForDashboard>()
                .ForMember(dest => dest.ReportText, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserSendReport.UserName))
                .ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.SubComment.Text))
                .ForMember(dest => dest.DateSentComment, opt => opt.MapFrom(src => src.SubComment.DateComment))
                .ForMember(dest => dest.UserNameSentComment, opt => opt.MapFrom(src => src.UserSendReport.UserName));

            CreateMap<ReportCourseInput, ReportCourse>();
            CreateMap<ReportCourse, ReportCourseOutput>();
            CreateMap<ReportCourse, ReportCourseForDashboard>()
                .ForMember(dest => dest.ReportText, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserSendReport.UserName))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Course.Teacher.User.FullName));

            CreateMap<ReportMessageInput, ReportMessage>();
            CreateMap<ReportMessage, ReportMessageOutput>();
            CreateMap<ReportMessage, ReportMessageForDashboard>()
                .ForMember(dest => dest.ReportText, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserSendReport.UserName))
                .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.Message.Text))
                .ForMember(dest => dest.DateSentMessage, opt => opt.MapFrom(src => src.Message.DateSent))
                .ForMember(dest => dest.UserNameSentThisMessage, opt => opt.MapFrom(src => src.Message.Sender.UserName));

            CreateMap<ReportUserInput, ReportUser>();
            CreateMap<ReportUser, ReportUserOutput>();
            CreateMap<ReportUser, ShowReportUserForDashboard>()
                .ForMember(dest => dest.ReportText, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.UserSendReport, opt => opt.MapFrom(src => src.UserSendReport.UserName))
                .ForMember(dest => dest.UserReciveReport, opt => opt.MapFrom(src => src.UserReciveReport.UserName));
        }
    }
}