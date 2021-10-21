using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using DAL.Entities.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.SubComment.Inputs;
using Model.SubComment.Outputs;
using Services;

namespace API.Controllers
{
    public class SubCommentController:BaseController
    {
        private readonly IMapper _mapper;
        private readonly ISubCommentService _isubCommentService;
        private readonly IAccountService _accountService;
        public SubCommentController(IMapper mapper, ISubCommentService isubCommentService, IAccountService accountService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _isubCommentService = isubCommentService;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<SubCommentOutput>> Get(int Id)
        {
            var comment = await _isubCommentService.GetSubCommmnetAsync(Id);
            if (comment is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<SubComment,SubCommentOutput>(comment));
        }
        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult> Create(SubCommentCreateInput comment)
        {
            var userTask = _accountService.GetUserByUserClaim(HttpContext.User);
            var commentToCreate = _mapper.Map<SubCommentCreateInput, SubComment>(comment);
            commentToCreate.UserId = (await userTask).Id;
            if (await _isubCommentService.CreateAsync(commentToCreate))
                return Ok("Done");
            return BadRequest();
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<ActionResult> Update(SubCommentUpdateInput comment)
        {
            var userTask = _accountService.GetUserByUserClaim(HttpContext.User);
            var commentToCreate = _mapper.Map<SubCommentUpdateInput, SubComment>(comment);
            if (!await _isubCommentService.IsTheOwner((await userTask).Id, comment.Id))
            {
                return Unauthorized();
            }
            if (await _isubCommentService.UpdateAsync(commentToCreate))
                return Ok("Done");
            return BadRequest();
        }
        [Authorize]
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete(int Id)
        {
            var userTask = _accountService.GetUserByUserClaim(HttpContext.User);
            if (!await _isubCommentService.IsTheOwner((await userTask).Id, Id))
            {
                return Unauthorized();
            }
            if (await _isubCommentService.DeleteAsync(Id))
                return Ok("Done");
            return BadRequest();
        }
    }
}