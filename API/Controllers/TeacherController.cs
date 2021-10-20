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
        private readonly ICourseService _ICourseService;
        private readonly ITeacherService _ITeacherService;
        private readonly IAccountService _IAccountService;
        private readonly IMapper _Mapper;
        public TeacherController(ICourseService CourseService, ITeacherService TeacherService, IAccountService AccountService, IMapper mapper)
        {
            _ICourseService = CourseService;
            _ITeacherService = TeacherService;
            _IAccountService = AccountService;
            _Mapper = mapper;
        }
        [HttpGet("{UserName}")]
        public async Task<ActionResult<TeacherOutput>> GetTeacherInfo(string UserName)
        {
            var teacher = await _ITeacherService.GetTeacherInfoAsync(UserName);
            if (teacher == null)
                return NotFound("no Teacher with " + UserName + " UserName found");
            return Ok(_Mapper.Map<Teacher, TeacherOutput>(teacher));
        }
        [HttpGet("{UserName}/Courses")]
        public async Task<List<TeacherCourseOutput>> GetTeacherCourses(string UserName) =>
            _Mapper.Map<List<Course>, List<TeacherCourseOutput>>(await _ITeacherService.GetTeacherCoursesAsync(UserName));

        [Authorize(Roles = "Teacher")]
        [HttpPost("Update")]
        public async Task<ActionResult> Update(TeacherUpdateInput teacher)
        {
            var userId=(await _IAccountService.GetUserByUserClaim(HttpContext.User)).Id;
            var techerid = await _ITeacherService.GetTeacherIdOrDefaultAsync(userId);
            if (default == techerid)
                return Unauthorized();
            var teacherToUpdate = _Mapper.Map<TeacherUpdateInput, Teacher>(teacher); 
            teacherToUpdate.Id = techerid;
            teacherToUpdate.UserId=userId;
            if (await _ITeacherService.UpdateTeacherInfoAsync(teacherToUpdate))
                return Ok("Done");
            return BadRequest();
        }
    }
}