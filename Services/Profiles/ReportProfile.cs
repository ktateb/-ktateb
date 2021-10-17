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
            CreateMap<ReportCourseInput, ReportCourse>();
            CreateMap<ReportCourse, ReportCourseOutput>();
            CreateMap<ReportMessageInput, ReportMessage>();
            CreateMap<ReportMessage, ReportMessageOutput>();
            CreateMap<ReportUserInput, ReportUser>();
            CreateMap<ReportUser, ReportUserOutput>();
        }
    }
}