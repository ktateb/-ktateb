using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
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
        public async Task<ActionResult<TeacherOutput>> GetTeacherInfo(string UserName)
        {
            var teacher = await _teacherService.GetTeacherInfoAsync(UserName);
            if (teacher == null)
                return NotFound("no Teacher with " + UserName + " UserName found");
            return Ok(_mapper.Map<Teacher, TeacherOutput>(teacher));
        }
        [HttpGet("{UserName}/Courses")]
        public async Task<List<TeacherCourseOutput>> GetTeacherCourses(string UserName) =>
            _mapper.Map<List<Course>, List<TeacherCourseOutput>>(await _teacherService.GetTeacherCoursesAsync(UserName));

        [Authorize(Roles = "Teacher")]
        [HttpPost("Update")]
        public async Task<ActionResult> Update(TeacherUpdateInput teacher)
        {
            var userId=(await _accountService.GetUserByUserClaim(HttpContext.User)).Id;
            var techerid = await _teacherService.GetTeacherIdOrDefaultAsync(userId);
            if (default == techerid)
                return Unauthorized();
            var teacherToUpdate = _mapper.Map<TeacherUpdateInput, Teacher>(teacher); 
            teacherToUpdate.Id = techerid;
            teacherToUpdate.UserId=userId;
            if (await _teacherService.UpdateTeacherInfoAsync(teacherToUpdate))
                return Ok("Done");
            return BadRequest();
        }
    }
}