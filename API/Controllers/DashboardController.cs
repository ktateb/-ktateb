using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using DAL.Entities.Identity;
using DAL.Entities.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Report.Comment.Outputs;
using Model.Report.Course.Outputs;
using Model.Report.Message.Outputs;
using Model.Report.User.Outputs;
using Model.Role.Inputs;
using Model.Role.Outputs;
using Model.User.Inputs;
using Model.User.Outputs;
using Services;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : BaseController
    {
        private readonly IMessageService _messageService;
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public DashboardController(IMessageService messageService, IDashboardService dashboardService, ITokenService tokenService, IMapper mapper)
        {
            _messageService = messageService;
            _dashboardService = dashboardService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("Adduser")]
        public async Task<ActionResult<UserOutput>> AddUser(UserRegister input)
        {
            var dbRecord = await _dashboardService.GetUserByNameAsync(input.UserName);
            if (dbRecord != null)
                return BadRequest("This username is used by another user");
            dbRecord = await _dashboardService.GetUserByEmailAsync(input.Email);
            if (dbRecord != null)
                return BadRequest("This email is used by another user");
            var result = await _dashboardService.RegisterUser(input);
            if (!result)
                return BadRequest("Something wrong when you regester");
            dbRecord = await _dashboardService.GetUserByNameAsync(input.UserName);
            var data = _mapper.Map<User, UserOutput>(dbRecord);
            data.Token = await _tokenService.CreateToken(dbRecord);
            return Ok(data);
        }

        [HttpGet("User")]
        public async Task<ActionResult<UsersOutput>> GetUser(string userName)
        {
            var dbRecord = await _dashboardService.GetUserByNameAsync(userName);
            if (dbRecord == null)
                return NotFound("User not found");
            var data = _mapper.Map<User, UsersOutput>(dbRecord);
            return Ok(data);
        }

        [HttpGet("UserByEmail")]
        public async Task<ActionResult<UsersOutput>> GetUserByEmail(string email)
        {
            var dbRecord = await _dashboardService.GetUserByEmailAsync(email);
            if (dbRecord == null)
                return NotFound("User not found");
            var data = _mapper.Map<User, UsersOutput>(dbRecord);
            return Ok(data);
        }

        [HttpGet("Users")]
        public async Task<ActionResult<List<UsersOutput>>> GetUsers() =>
            Ok(_mapper.Map<List<User>, List<UsersOutput>>(await _dashboardService.GetUsers()));

        [HttpPost("UpdateUser")]
        public async Task<ActionResult> UpdateUser(UserUpdate input)
        {
            var user = await _dashboardService.GetUserByIdAsync(input.Id);
            if (user == null)
                return NotFound("User not exist");

            if (await _dashboardService.UpdateUser(_mapper.Map<UserUpdate, User>(input, user)))
                return Ok("Done");
            return BadRequest("Error happen when update user");
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await _dashboardService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not exist");

            if (await _dashboardService.DeleteUser(id))
                return Ok("Done");
            return BadRequest("Error happen when delete user");
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePassword input)
        {
            var user = await _dashboardService.GetUserByNameAsync(input.UserName);
            if (user == null)
                return NotFound("User not exist");

            if (await _dashboardService.CheckPassword(user, input.CurrentPassword) == false)
                return BadRequest("Please enter your correct password");
            await _dashboardService.ChangePassword(user, input.CurrentPassword, input.Password);
            return Ok("Done");
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult<RoleOutput>> CreateRoleAsync(RoleInput inputRole)
        {
            var dbRecord = await _dashboardService.GetRoleByNameAsync(inputRole.Name);
            if (dbRecord != null)
                return BadRequest("This role is exist");
            var role = _mapper.Map<RoleInput, Role>(inputRole);
            await _dashboardService.CreateRoleAsync(role);
            return Ok(_mapper.Map<Role, RoleOutput>(role));
        }

        [HttpPost("UpdateRole")]
        public async Task<ActionResult<RoleOutput>> UpdateRoleAsync(RoleUpdate inputRole)
        {
            var dbRecord = await _dashboardService.GetRoleByIdAsync(inputRole.Id);
            if (dbRecord == null)
                return BadRequest("This role is not exist");
            if (await _dashboardService.GetRoleByNameAsync(inputRole.Name) != null)
                return BadRequest($"This role {inputRole.Name} is exist");
            var role = _mapper.Map<RoleUpdate, Role>(inputRole, dbRecord);
            await _dashboardService.UpdateRoleAsync(role);
            return Ok(_mapper.Map<Role, RoleOutput>(role));
        }

        [HttpPost("Roles")]
        public async Task<ActionResult<List<string>>> GetRolesByUserIdAsync(string id) =>
             Ok(await _dashboardService.GetRolesByUserIdAsync(id));

        [HttpPost("AddRoleToUser")]
        public async Task<ActionResult> AddRoleToUserAsync(AddRoleToUserInput input)
        {
            var dbRecordUser = await _dashboardService.GetUserByNameAsync(input.UserName);
            if (dbRecordUser == null)
                return NotFound("User not exist");
            var dbRecordRole = await _dashboardService.GetRoleByNameAsync(input.RoleName);
            if (dbRecordRole == null)
                return NotFound("Role not exist");
            await _dashboardService.AddRoleToUserAsync(dbRecordUser, input.RoleName);
            return Ok("Done");
        }

        [HttpPost("DeleteRoleFromUser")]
        public async Task<ActionResult> DeleteRoleInUser(AddRoleToUserInput input)
        {
            var dbRecordUser = await _dashboardService.GetUserByNameAsync(input.UserName);
            if (dbRecordUser == null)
                return NotFound("User not exist");
            var dbRecordRole = await _dashboardService.GetRoleByNameAsync(input.RoleName);
            if (dbRecordRole == null)
                return NotFound("Role not exist");
            await _dashboardService.DeleteRoleInUser(dbRecordUser, dbRecordRole.Name);
            return Ok("Done");
        }

        [HttpGet("Role")]
        public async Task<ActionResult<RoleOutput>> GetRoleByNameAsync(string name) =>
            Ok(_mapper.Map<Role, RoleOutput>(await _dashboardService.GetRoleByNameAsync(name)));

        [HttpGet("AllRoles")]
        public async Task<List<RoleOutput>> GetAllRoles() =>
            _mapper.Map<List<Role>, List<RoleOutput>>(await _dashboardService.GetRolesAsync());

        [HttpGet("ShowReportedMessages")]
        public async Task<ActionResult<List<ReportMessageForDashboard>>> ShowReportedMessages() =>
            Ok(_mapper.Map<List<ReportMessage>, List<ReportMessageForDashboard>>(await _dashboardService.ShowReportedMessages()));

        [HttpGet("ShowReportedMessage/{messageId}")]
        public async Task<ActionResult<List<ReportMessageForDashboard>>> ShowReportedMessage(int messageId) =>
            Ok(_mapper.Map<List<ReportMessage>, List<ReportMessageForDashboard>>(await _dashboardService.ShowReportedMessage(messageId)));

        [HttpGet("ShowReportedComments")]
        public async Task<ActionResult<List<ReportCommentForDashboard>>> ShowReportedComments() =>
            Ok(_mapper.Map<List<ReportComment>, List<ReportCommentForDashboard>>(await _dashboardService.ShowReportedComments()));

        [HttpGet("ShowReportedComment/{commentId}")]
        public async Task<ActionResult<List<ReportCommentForDashboard>>> ShowReportedComment(int commentId) =>
            Ok(_mapper.Map<List<ReportComment>, List<ReportCommentForDashboard>>(await _dashboardService.ShowReportedComment(commentId)));

        [HttpGet("ShowReportedCourses")]
        public async Task<ActionResult<List<ReportCourseForDashboard>>> ShowReportedCourses() =>
            Ok(_mapper.Map<List<ReportCourse>, List<ReportCourseForDashboard>>(await _dashboardService.ShowReportedCourses()));

        [HttpGet("ShowReportedCourses/{courseId}")]
        public async Task<ActionResult<List<ReportCourseForDashboard>>> ShowReportedCourse(int courseId) =>
            Ok(_mapper.Map<List<ReportCourse>, List<ReportCourseForDashboard>>(await _dashboardService.ShowReportedCourse(courseId)));

        [HttpGet("ShowReportedUsers")]
        public async Task<ActionResult<List<ShowReportUserForDashboard>>> ShowReportedUsers() =>
            Ok(_mapper.Map<List<ReportUser>, List<ShowReportUserForDashboard>>(await _dashboardService.ShowReportedUsers()));

        [HttpGet("ShowReportedUser/{userId}")]
        public async Task<ActionResult<List<ShowReportUserForDashboard>>> ShowReportedCourse(string userId) =>
            Ok(_mapper.Map<List<ReportUser>, List<ShowReportUserForDashboard>>(await _dashboardService.ShowReportedUser(userId)));

        [HttpPost("DeleteMessage/{messageId}")]
        public async Task<ActionResult> DeleteMessage(int messageId)
        {
            var dbRecord = await _messageService.GetMessage(messageId);
            if (dbRecord == null)
                return NotFound("This message is not exist");
            await _dashboardService.DeleteMessage(messageId);
            return Ok("Done");
        }
    }
}