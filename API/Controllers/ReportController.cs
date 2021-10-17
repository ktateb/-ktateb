using System;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using DAL.Entities.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Report.Comment.Inputs;
using Model.Report.Comment.Outputs;
using Model.Report.Course.Inputs;
using Model.Report.Course.Outputs;
using Model.Report.Message.Inputs;
using Model.Report.Message.Outputs;
using Model.Report.User.Inputs;
using Model.Report.User.Outputs;
using Services;

namespace API.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly IAccountService _accountService;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        public ReportController(IAccountService accountService, IMessageService messageService, IReportService reportService, IMapper mapper)
        {
            _mapper = mapper;
            _accountService = accountService;
            _messageService = messageService;
            _reportService = reportService;
        }

        [HttpPost("ReportMessage")]
        public async Task<ActionResult<ReportMessageOutput>> ReportMessage(ReportMessageInput input)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null)
                return Unauthorized("User is Unauthorized");
            var message = await _messageService.GetMessage(input.MessageId);
            if (message == null)
                return NotFound("This message is not exist");
            if (message.SenderId == user.Id)
                return BadRequest(@"You can't report you message Lol");

            var report = _mapper.Map<ReportMessageInput, ReportMessage>(input);
            report.DateReport = DateTime.UtcNow;
            report.UserId = user.Id;
            await _reportService.ReportMessage(report);
            return Ok(_mapper.Map<ReportMessage, ReportMessageOutput>(report));
        }

        [HttpPost("ReportComment")]
        public async Task<ActionResult<ReportCommentOutput>> ReportComment(ReportCommentInput input)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null)
                return Unauthorized("User is Unauthorized");
            var report = _mapper.Map<ReportCommentInput, ReportComment>(input);
            report.UserId = user.Id;
            report.DateReport = DateTime.UtcNow;
            await _reportService.ReportComment(report);
            return Ok(_mapper.Map<ReportComment, ReportCommentOutput>(report));
        }

        [HttpPost("ReportCourse")]
        public async Task<ActionResult<ReportCourseOutput>> ReportCourse(ReportCourseInput input)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null)
                return Unauthorized("User is Unauthorized");
            var report = _mapper.Map<ReportCourseInput, ReportCourse>(input);
            report.DateReport = DateTime.UtcNow;
            report.UserId = user.Id;
            await _reportService.ReportCourse(report);
            return Ok(_mapper.Map<ReportCourse, ReportCourseOutput>(report));
        }

        [HttpPost("ReportUser")]
        public async Task<ActionResult<ReportUserOutput>> ReportUser(ReportUserInput input)
        {
            var user = await _accountService.GetUserByUserClaim(HttpContext.User);
            if (user == null)
                return Unauthorized("User is Unauthorized");
            if (user.Id == input.UserReciveReportId)
                return BadRequest(@"You can't report yourself Lol");
            var report = _mapper.Map<ReportUserInput, ReportUser>(input);
            report.DateReport = DateTime.UtcNow;
            report.UserSendReportId = user.Id;
            await _reportService.ReportUser(report);
            return Ok(_mapper.Map<ReportUser, ReportUserOutput>(report));
        }
    }
}