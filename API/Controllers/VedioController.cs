using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Course.Inputs;
using Model.Vedio;
using Services;
using Services.Services;

namespace API.Controllers
{
    public class VedioController : BaseController
    {
        private readonly ITeacherService _TeacherService;
        private readonly IAccountService _accountService;
        private readonly IVedioService _VedioService;
        private readonly IMapper _mapper;
        private readonly IStudentWatchesService _iStudentWatchesService;
        public VedioController(IStudentWatchesService iStudentWatchesService, ITeacherService TeacherService, IAccountService accountService, IVedioService VedioService, IMapper mapper)
        {
            _iStudentWatchesService = iStudentWatchesService;
            _accountService = accountService;
            _TeacherService = TeacherService;
            _VedioService = VedioService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("{Id}/isWatchedByMe")]
        public async Task<ActionResult<ResultService<bool>>> isWatchedByMe(int Id) =>
              GetResult<bool>(await _iStudentWatchesService.isWatchedByMe(Id, (await _accountService.GetUserByUserClaim(HttpContext.User)).Id));

        [Authorize(Roles = "Teacher")]
        [HttpPost("Create")]
        public async Task<ActionResult<ResultService<VedioOutput>>> Create(VedioInput Input) =>
                GetResult<VedioOutput>(await _VedioService.Create(Input, await _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id)));
        [Authorize(Roles = "Teacher")]
        [HttpPost("Update")]
        public async Task<ActionResult<ResultService<VedioOutput>>> Update(VedioUpdateInput Input) =>
        GetResult<VedioOutput>(await _VedioService.Update(Input, await _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id)));


        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Teacher")]
        [HttpPost("{Id}/UploadeVedio")]
        public async Task<ActionResult<ResultService<VedioOutput>>> UploadeVedioAsync(int Id, [FromForm] CourseFile input) =>
             GetResult<VedioOutput>(await _VedioService.UploadeVedioAsync(Id, input, await _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id)));


        [Authorize(Roles = "Teacher")]
        [HttpPost("{Id}/UploadeImage")]
        public async Task<ActionResult<ResultService<VedioOutput>>> UploadeImageAsync(int Id, [FromForm] CourseFile input) =>
         GetResult<VedioOutput>(await _VedioService.UploadeImageAsync(Id, input, await _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id)));

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult<ResultService<VedioOutput>>> VedioInfo(int Id) =>
            GetResult<VedioOutput>(await _VedioService.VedioInfo(Id, (await _accountService.GetUserByUserClaim(HttpContext.User)).Id));

    }
}