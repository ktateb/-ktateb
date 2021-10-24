// using AutoMapper;
// using Microsoft.AspNetCore.Mvc;
// using Model.Comment.Outputs;
// using Services;
// using Microsoft.AspNetCore.Authorization;
// using API.Controllers.Common;
// using System.Threading.Tasks;
// using Model.Comment.Inputs;
// using DAL.Entities.Comments;
// using System;
// using Model.SubComment.Outputs;
// using System.Collections.Generic;
// using Model.Helper;
// using API.Helpers;
// using Services.Services;
// using API.Extensions;

// namespace API.Controllers
// {
//     public class CommentController : BaseController
//     {
//         private readonly IMapper _mapper;
//         private readonly ICommentService _iCommentService;
//         private readonly IAccountService _accountService;
//         public CommentController(IMapper mapper, ICommentService iCommentService, IAccountService accountService)
//         {
//             _accountService = accountService;
//             _mapper = mapper;
//             _iCommentService = iCommentService;
//         }
//         [HttpPost("{Id}/SubComments")]
//         public async Task<List<SubCommentOutput>> Get(int Id, Paging Params)
//         {
//             var subcomments = await _iCommentService.GetSubCommentsAsync(Id, Params);
//             Response.AddPagination(subcomments.CurrentPage, subcomments.ItemsPerPage, subcomments.TotalItems, subcomments.TotalPages);
//             return _mapper.Map<List<SubComment>, List<SubCommentOutput>>(subcomments);
//         }
//         [HttpGet("{Id}")]
//         public async Task<ActionResult<CommentOutput>> Get(int Id)
//         {
//             var comment = await _iCommentService.GetCommmnetAsync(Id);
//             return this.GetResult<CommentOutput>(comment);
//         }
//         [Authorize]
//         [HttpPost("Create")]
//         public async Task<ActionResult<string>> Create(CommentCreateInput comment)
//         {
//             var userTask = _accountService.GetUserByUserClaim(HttpContext.User);
//             return GetResult<string>(await _iCommentService.CreateAsync(comment, await userTask));
//         }

//         [Authorize]
//         [HttpPost("update")]
//         public async Task<ActionResult<String>> Update(CommentUpdateInput comment)
//         {
//             var userTask = _accountService.GetUserByUserClaim(HttpContext.User);
//             return GetResult<string>(await _iCommentService.UpdateAsync(comment, await userTask));
//         }
//         [Authorize]
//         [HttpPost("Delete")]
//         public async Task<ActionResult<string>> Delete(int Id)
//         {
//             var userTask = _accountService.GetUserByUserClaim(HttpContext.User); 
//             return GetResult<string>(await _iCommentService.DeleteAsync(Id, await userTask)); 
//         }
//     }
// }