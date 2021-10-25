using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using Common.Services;
using DAL.Entities.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.SubComment.Inputs;
using Model.SubComment.Outputs;
using Services;

namespace API.Controllers
{
    public class SubCommentController : BaseController
    { 
        private readonly ISubCommentService _isubCommentService;
        private readonly IAccountService _accountService;
        public SubCommentController(IMapper mapper, ISubCommentService isubCommentService, IAccountService accountService)
        {
            _accountService = accountService; 
            _isubCommentService = isubCommentService;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<ResultService<SubCommentOutput>>> Get(int Id) =>
             GetResult<SubCommentOutput>(await _isubCommentService.GetSubCommmnetAsync(Id));
        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult<ResultService<SubCommentOutput>>> Create(SubCommentCreateInput comment) =>
             GetResult<SubCommentOutput>(await _isubCommentService.CreateAsync(comment, await _accountService.GetUserByUserClaim(HttpContext.User)));
        [Authorize]
        [HttpPost("update")]
        public async Task<ActionResult<ResultService<SubCommentOutput>>> Update(SubCommentUpdateInput comment) =>
                   GetResult<SubCommentOutput>(await _isubCommentService.UpdateAsync(comment, await _accountService.GetUserByUserClaim(HttpContext.User)));
        [Authorize]
        [HttpPost("Delete")]
        public async Task<ActionResult<ResultService<bool>>> Delete(int Id) =>
             GetResult<bool>(await _isubCommentService.DeleteAsync(Id, await _accountService.GetUserByUserClaim(HttpContext.User)));

    }
}