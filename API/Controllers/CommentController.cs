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
using API.Extensions;
using Common.Services;

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
        public async Task<ActionResult<ResultService<CommentOutput>>> Get(int Id) =>
            GetResult<CommentOutput>(await _iCommentService.GetCommmnetAsync(Id));

        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult<ResultService<CommentOutput>>> Create(CommentCreateInput comment) =>
            GetResult<CommentOutput>(await _iCommentService.CreateAsync(comment, await _accountService.GetUserByUserClaim(HttpContext.User)));


        [Authorize]
        [HttpPost("update")]
        public async Task<ActionResult<ResultService<CommentOutput>>> Update(CommentUpdateInput comment) =>
            GetResult<CommentOutput>(await _iCommentService.UpdateAsync(comment, await _accountService.GetUserByUserClaim(HttpContext.User)));

        [Authorize]
        [HttpPost("Delete")]
        public async Task<ActionResult<ResultService<bool>>> Delete(int Id) =>
            GetResult<bool>(await _iCommentService.DeleteAsync(Id, await _accountService.GetUserByUserClaim(HttpContext.User)));

    }
}