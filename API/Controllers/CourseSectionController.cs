using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model.CourseSection.Outputs;
using Model.CourseSection.Inputs;
using Services;
using DAL.Entities.Courses;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class CourseSectionController : BaseController
    {
        private readonly ICourseService _CourseService;
        private readonly ICourseSectionService _CourseSectionService;
        private readonly IAccountService _accountService;
        private readonly ITeacherService _TeacherService;
        private readonly IMapper _mapper;
        public CourseSectionController(ICourseService CourseService, ICourseSectionService CourseSectionService, IAccountService accountService, ITeacherService TeacherService, IMapper mapper)
        {
            _accountService = accountService;
            _CourseSectionService = CourseSectionService;
            _CourseService = CourseService;
            _TeacherService = TeacherService;
            _mapper = mapper;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<CourseSectionOutput>> GetCourseSection(int Id)
        {
            var Section = await _CourseSectionService.GetSectionAsync(Id);
            if (Section is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CourseSection, CourseSectionOutput>(Section));
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("Create")]
        public async Task<ActionResult> Create(CourseSectionCreateInput SectionInput)
        {
            var CoursetecherIdTask = _CourseService.GetTeacherIdOrDefultAsync(SectionInput.CourseId);
            var techerId = await _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id);
            var CoursetecherId = await CoursetecherIdTask;
            if (CoursetecherId == default)
            {
                return NotFound();
            }
            if (techerId != CoursetecherId)
            {
                return Unauthorized("You are not the Owner");
            }
            if (await _CourseSectionService.CreateSectionInfoAsync(_mapper.Map<CourseSectionCreateInput, CourseSection>(SectionInput)))
                return Ok("Done");
            return BadRequest();
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost("Update")]
        public async Task<ActionResult> Update(CourseSectionUpdateInput SectionInput)
        {
            var SectionTecherIdTask = _CourseSectionService.GetTeacerIdAsync(SectionInput.SectionId);
            var CourseIdTask = _CourseSectionService.GetCourseIdAsync(SectionInput.SectionId);
            var authTecherIdtask = _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id);

            var SectionTecherId = await SectionTecherIdTask;
            var authTecherId = await authTecherIdtask;
            if (SectionTecherId == default)
            {
                return NotFound();
            }

            if (authTecherId != SectionTecherId)
            {
                return Unauthorized("You are not the Owner");
            }
            var Section = _mapper.Map<CourseSectionUpdateInput, CourseSection>(SectionInput);
            Section.CourseId = await CourseIdTask;
            if (await _CourseSectionService.UpdateSectionInfoAsync(Section))
                return Ok("Done");
            return BadRequest();
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(int Id)
        {
            var sectionTecherIdTask = _CourseSectionService.GetTeacerIdAsync(Id);
            var courseIdTask = _CourseSectionService.GetCourseIdAsync(Id);
            var authTecherIdtask = _TeacherService.GetTeacherIdOrDefaultAsync((await _accountService.GetUserByUserClaim(HttpContext.User)).Id);

            var sectionTecherId = await sectionTecherIdTask;
            var authTecherId = await authTecherIdtask;
            if (sectionTecherId == default)
            {
                return NotFound("section Not Found");
            }
            if (authTecherId != sectionTecherId)
            {
                return Unauthorized("You are not the Owner");
            }
            if (await _CourseService.HasStudentAsync(await courseIdTask))
            {
                return BadRequest("this Course has student");
            }
            if (await _CourseSectionService.DeleteSectionAsync(Id))
                return Ok("Done");
            return BadRequest();
        }
    }
}