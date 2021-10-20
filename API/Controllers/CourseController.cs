using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using DAL.Entities.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Course.Inputs;
using Model.Course.Outputs;
using Model.CourseSection.Outputs;
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
        public async Task<ActionResult<CourseOutput>> GetCourseInfo(int Id)
        {
            var Course = _mapper.Map<Course, CourseOutput>(await _CourseService.GetCourseInfoAsync(Id));

            if (Course == null)
            {
                return NotFound("Course not found");
            }
            return Ok(Course);
        }
        [HttpGet("{Id}/Sections")]
        public async Task<ActionResult<List<CourseSectionOutput>>> GetCourseSection(int Id)
        {
            if (!(await _CourseService.IsExistAsync(Id)))
            {
                return NotFound("Course not found");
            }
            return Ok(_mapper.Map<List<CourseSection>, List<CourseSectionOutput>>(await _CourseService.GetCourseSectionAsync(Id)));
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost("Create")]
        public async Task<ActionResult> Create(CourseCreateInput Course)
        {
            var techerId = await _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id);
            Course course = _mapper.Map<CourseCreateInput, Course>(Course);
            course.TeacherId = techerId;
            course.CreatedDate = System.DateTime.Now;
            if (await _CourseService.CreateCoursesAsync(course))
                return Ok("Done");
            return BadRequest();

        }
        [Authorize(Roles = "Teacher")]
        [HttpPost("Update")]
        public async Task<ActionResult> Update(CourseUpdateInput CourseInput)
        {
            var CoursetecherIdTask = _CourseService.GetTeacherIdOrDefultAsync(CourseInput.Id);
            var createdateTask = _CourseService.GetCourseCreatedDateCourseAsync(CourseInput.Id);
            var AuthtecherIdTask = _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id);

            var CoursetecherId = await CoursetecherIdTask;
            var AuthtecherId = await AuthtecherIdTask;
            if (CoursetecherId == default)
            {
                return NotFound("Course not found");
            }
            if (CoursetecherId != AuthtecherId)
            {
                return Unauthorized();
            }
            Course course = _mapper.Map<CourseUpdateInput, Course>(CourseInput);
            course.TeacherId = CoursetecherId;
            course.CreatedDate = await createdateTask;
            if (await _CourseService.UpdateCourseInfoAsync(course))
                return Ok("Done");
            return BadRequest();
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(int Id)
        {

            var CoursetecherIdTask = _CourseService.GetTeacherIdOrDefultAsync(Id);
            var AuthtecherIdTask = _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id);
            var HasStudentTask= _CourseService.HasStudentAsync(Id);
            var CoursetecherId = await CoursetecherIdTask;
            var AuthtecherId = await AuthtecherIdTask;
            if (default == CoursetecherId)
            {
                return NotFound("Course not found");
            }
            if (CoursetecherId != AuthtecherId)
            {
                return Unauthorized("You are not the Owner of this course");
            } 
            if (await HasStudentTask)
            {
                return BadRequest("this course Has Students");
            }
            if (await _CourseService.DeleteCourseAsync(Id))
                return Ok("Done");
            return BadRequest();
        } 
    }
}