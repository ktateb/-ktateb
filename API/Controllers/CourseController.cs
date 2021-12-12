using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using API.Helpers;
using AutoMapper;
using Common.Services;
using DAL.Entities.Comments;
using DAL.Entities.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Model.Comment.Outputs;
using Model.Course.Inputs;
using Model.Course.Outputs;
using Model.CourseSection.Outputs;
using Model.Helper;
using Services;

namespace API.Controllers
{
    public class CourseController : BaseController
    {
        private readonly ICourseService _CourseService;
        private readonly IAccountService _accountService;
        private readonly ITeacherService _TeacherService;
        private readonly IMapper _mapper;
        public CourseController(ICourseService CourseService, IAccountService accountService, ITeacherService TeacherService, IMapper mapper)
        {
            _accountService = accountService;
            _CourseService = CourseService;
            _TeacherService = TeacherService;
            _mapper = mapper;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<ResultService<CourseOutput>>> GetCourseInfo(int Id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return GetResult<CourseOutput>(await _CourseService.GetCourseInfoAsync(Id, (await _accountService.GetUserByUserClaim(HttpContext.User)).Id));
            else
                return GetResult<CourseOutput>(await _CourseService.GetCourseInfoAsync(Id));

        }
        [Authorize(Roles = "Teacher")]
        [HttpPost("{CourseId}/PriceHistory")]
        public async Task<ActionResult<ResultService<List<PriceHistoryOutput>>>> UpdatePrice(int CourseId) =>
            GetResult<List<PriceHistoryOutput>>(await _CourseService.GetPriceHistoryAsync(CourseId));

        [HttpGet("{CourseId}/Sections")]
        public async Task<ActionResult<ResultService<List<CourseSectionOutput>>>> GetCourseSection(int CourseId) =>
            GetResult<List<CourseSectionOutput>>(await _CourseService.GetCourseSectionAsync(CourseId));
        [HttpPost("{CourseId}/Comments")]
        public async Task<List<CommentOutput>> Get(int CourseId, Paging Params)
        {
            var comments = await _CourseService.GetCommentsAsync(CourseId, Params);
            Response.AddPagination(comments.CurrentPage, comments.ItemsPerPage, comments.TotalItems, comments.TotalPages);
            return _mapper.Map<List<Comment>, List<CommentOutput>>(comments);
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost("Create")]
        public async Task<ActionResult<ResultService<CourseOutput>>> Create(CourseCreateInput Course) =>
            GetResult<CourseOutput>(await _CourseService.CreateCoursesAsync(Course, await _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id)));

        [Authorize]
        [HttpPost("{CourseId}/Regist")]
        public async Task<ActionResult<ResultService<bool>>> Regist(int CourseId) =>
            GetResult<bool>(await _CourseService.FreeRegistAsync(CourseId, (await _accountService.GetUserByUserClaim(HttpContext.User)).Id));

        [Authorize(Roles = "Teacher")]
        [HttpPost("Update")]
        public async Task<ActionResult<ResultService<bool>>> Update(CourseUpdateInput CourseInput) =>
             GetResult<bool>(await _CourseService.UpdateCourseInfoAsync(CourseInput, await _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id)));


        [Authorize(Roles = "Teacher")]
        [HttpDelete("Delete")]
        public async Task<ActionResult<ResultService<bool>>> Delete(int Id) =>
            GetResult<bool>(await _CourseService.DeleteCourseAsync(Id, await _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id)));

        [Authorize(Roles = "Teacher")]
        [HttpPost("{CourseId}/UpdatePrice")]
        public async Task<ActionResult<ResultService<bool>>> UpdatePrice(int CourseId, double newprice) =>
            GetResult<bool>(await _CourseService.CreateNewPriceHistory(CourseId, newprice, await _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id)));

    }
}
