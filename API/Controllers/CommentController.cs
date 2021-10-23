using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model.Comment.Outputs;
using Services;
using Microsoft.AspNetCore.Authorization;
using API.Controllers.Common;
using System.Threading.Tasks;
using Model.Comment.Inputs;
using DAL.Entities.Comments;
using System;
using Model.SubComment.Outputs;
using System.Collections.Generic;
using Model.Helper;
using API.Helpers;
using Services.Services;

namespace API.Controllers
{
    public class CommentController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ICommentService _iCommentService;
        private readonly IAccountService _accountService;
        public CommentController(IMapper mapper, ICommentService iCommentService, IAccountService accountService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _iCommentService = iCommentService;
        }
        [HttpPost("{Id}/SubComments")]
        public async Task<List<SubCommentOutput>> Get(int Id, Paging Params)
        {
            var subcomments = await _iCommentService.GetSubCommentsAsync(Id, Params);
            Response.AddPagination(subcomments.CurrentPage, subcomments.ItemsPerPage, subcomments.TotalItems, subcomments.TotalPages);
            return _mapper.Map<List<SubComment>, List<SubCommentOutput>>(subcomments);
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<CommentOutput>> Get(int Id)
        {
            var comment = await _iCommentService.GetCommmnetAsync(Id);
            if (comment.Code == ResultStatusCode.NotFound)
            {
                return NotFound(comment.Messege);
            }
            return Ok(comment.Result);
        }
        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult> Create(CommentCreateInput comment)
        {
            var userTask = _accountService.GetUserByUserClaim(HttpContext.User);
            var commentToCreate = _mapper.Map<CommentCreateInput, Comment>(comment);
            commentToCreate.UserId = (await userTask).Id;
            if (await _iCommentService.CreateAsync(commentToCreate))
                return Ok("Done");
            return BadRequest();
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<ActionResult> Update(CommentUpdateInput comment)
        {
            var userTask = _accountService.GetUserByUserClaim(HttpContext.User);
            var commentToCreate = _mapper.Map<CommentUpdateInput, Comment>(comment);
            var userid = (await userTask).Id;
            if (!await _iCommentService.IsTheOwner(userid, comment.Id))
            {
                return Unauthorized();
            }
            commentToCreate.UserId = (await userTask).Id;
            if (await _iCommentService.UpdateAsync(commentToCreate))
                return Ok("Done");
            return BadRequest();
        }
        [Authorize]
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete(int Id)
        {
            var userTask = _accountService.GetUserByUserClaim(HttpContext.User);
            if (!await _iCommentService.IsTheOwner((await userTask).Id, Id))
            {
                return Unauthorized();
            }
            if (await _iCommentService.DeleteAsync(Id))
                return Ok("Done");
            return BadRequest();
        }
    }
}