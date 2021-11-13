using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using Common.Services;
using DAL.Entities.Courses;
using DAL.Entities.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Course.Outputs;
using Model.Teacher.Inputs;
using Model.Teacher.Outputs;
using Services;

namespace API.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _teacherService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public TeacherController(ITeacherService teacherService, IAccountService accountService, IMapper mapper)
        {
            _teacherService = teacherService;
            _accountService = accountService;
            _mapper = mapper;
        }
        [HttpGet("{UserName}")]
        public async Task<ActionResult<ResultService<TeacherOutput>>> GetTeacherInfo(string UserName) =>
            GetResult<TeacherOutput>(await _teacherService.GetTeacherInfoAsync(UserName));

        [Authorize(Roles = "Teacher")]
        [HttpGet("Me")]
        public async Task<ActionResult<ResultService<TeacherOutput>>> Me() =>
             GetResult<TeacherOutput>(await _teacherService.GetTeacherInfoAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).UserName));

        [HttpGet("{UserName}/Courses")]
        public async Task<List<TeacherCourseOutput>> GetTeacherCourses(string UserName) =>
            _mapper.Map<List<Course>, List<TeacherCourseOutput>>(await _teacherService.GetTeacherCoursesAsync(UserName));

        [Authorize(Roles = "Teacher")]
        [HttpPost("Update")]
        public async Task<ActionResult<ResultService<TeacherOutput>>> Update(TeacherUpdateInput teacher) =>
            GetResult<TeacherOutput>(await _teacherService.UpdateTeacherInfoAsync(teacher, (await _accountService.GetUserByUserClaim(HttpContext.User)).Id));
    }
}