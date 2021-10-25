using System.Threading.Tasks;
using API.Controllers.Common;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Report.Comment.Inputs;
using Model.Report.Course.Inputs;
using Model.Report.Message.Inputs;
using Model.Report.User.Inputs;
using Services;

namespace API.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly IAccountService _accountService;
        public ReportController(IAccountService accountService, IReportService reportService)
        {
            _accountService = accountService;
            _reportService = reportService;
        }

        [HttpPost("ReportMessage")]
        public async Task<ActionResult<ResultService<bool>>> ReportMessage(ReportMessageInput input) =>
            GetResult(await _reportService.ReportMessage(input, await _accountService.GetUserByUserClaim(HttpContext.User)));

        [HttpPost("ReportComment")]
        public async Task<ActionResult<ResultService<bool>>> ReportComment(ReportCommentInput input) =>
            GetResult(await _reportService.ReportComment(input, await _accountService.GetUserByUserClaim(HttpContext.User)));

        [HttpPost("ReportSubComment")]
        public async Task<ActionResult<ResultService<bool>>> ReportSubComment(ReportCommentInput input) =>
            GetResult(await _reportService.ReportSubComment(input, await _accountService.GetUserByUserClaim(HttpContext.User)));

        [HttpPost("ReportCourse")]
        public async Task<ActionResult<ResultService<bool>>> ReportCourse(ReportCourseInput input) =>
            GetResult(await _reportService.ReportCourse(input, await _accountService.GetUserByUserClaim(HttpContext.User)));

        [HttpPost("ReportUser")]
        public async Task<ActionResult<ResultService<bool>>> ReportUser(ReportUserInput input) =>
            GetResult(await _reportService.ReportUser(input, await _accountService.GetUserByUserClaim(HttpContext.User)));
    }
}