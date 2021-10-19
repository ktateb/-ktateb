using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.Entities.Comments;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using DAL.Entities.Identity.enums;
using DAL.Entities.Messages;
using DAL.Entities.Reports;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Helper;
using Model.Report.Message.Inputs;
using Model.Role.Inputs;
using Model.User.Inputs;

namespace Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IGenericRepository<ReportComment> _reportCommentRepository;
        private readonly IGenericRepository<ReportUser> _reportUserRepository;
        private readonly IGenericRepository<ReportMessage> _reportMessageRepository;
        private readonly IGenericRepository<ReportCourse> _reportCourseRepository;
        private readonly IGenericRepository<SubComment> _subCommentRepository;

        public DashboardService(IIdentityRepository identityRepository
            , IGenericRepository<Comment> commentRepository, IGenericRepository<Message> messageRepository
            , IGenericRepository<Course> courseRepository, IGenericRepository<ReportComment> reportCommentRepository
            , IGenericRepository<ReportUser> reportUserRepository, IGenericRepository<ReportMessage> reportMessageRepository
            , IGenericRepository<ReportCourse> reportCourseRepository, IGenericRepository<SubComment> subCommentRepository)
        {
            _commentRepository = commentRepository;
            _identityRepository = identityRepository;
            _messageRepository = messageRepository;
            _courseRepository = courseRepository;
            _reportCommentRepository = reportCommentRepository;
            _reportUserRepository = reportUserRepository;
            _reportMessageRepository = reportMessageRepository;
            _reportCourseRepository = reportCourseRepository;
            _subCommentRepository = subCommentRepository;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var dbRecord = await _identityRepository.GetUserByEmailAsync(email);
            if (dbRecord != null)
            {
                var dbRecordNameRole = await _identityRepository.GetRolesByUserIdAsync(dbRecord.Id);
                List<Role> roles = new();
                foreach (var roleName in dbRecordNameRole)
                {
                    var role = await _identityRepository.GetRoleByNameAsync(roleName);
                    roles.Add(role);
                }
                dbRecord.Roles = roles;
            }
            return dbRecord;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var dbRecord = await _identityRepository.GetUserByIdAsync(id);
            if (dbRecord != null)
            {
                var dbRecordNameRole = await _identityRepository.GetRolesByUserIdAsync(dbRecord.Id);
                List<Role> roles = new();
                foreach (var roleName in dbRecordNameRole)
                {
                    var role = await _identityRepository.GetRoleByNameAsync(roleName);
                    roles.Add(role);
                }
                dbRecord.Roles = roles;
            }
            return dbRecord;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var dbRecord = await _identityRepository.GetUserByNameAsync(name);
            if (dbRecord != null)
            {
                var dbRecordNameRole = await _identityRepository.GetRolesByUserIdAsync(dbRecord.Id);
                List<Role> roles = new();
                foreach (var roleName in dbRecordNameRole)
                {
                    var role = await _identityRepository.GetRoleByNameAsync(roleName);
                    roles.Add(role);
                }
                dbRecord.Roles = roles;
            }
            return dbRecord;
        }

        public async Task<List<string>> GetRolesByUserIdAsync(string id) =>
            await _identityRepository.GetRolesByUserIdAsync(id);

        public async Task<bool> RegisterUser(UserRegister input)
        {
            var user = new User
            {
                UserName = input.UserName,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Birthday = input.Birthday,
                StartRegisterDate = DateTime.UtcNow,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
                Country = input.Country,
            };

            var result = await _identityRepository.CreateUserAsync(user, input.Password);
            if (!result)
                return false;

            var roles = await _identityRepository.GetRolesByUserIdAsync(user.Id);
            if (roles.Count == 0)
            {
                roles.Add(Roles.Student.ToString());
            }
            foreach (var role in roles)
            {
                var dbRecordRole = await _identityRepository.GetRoleByNameAsync(role);
                if (dbRecordRole == null)
                {
                    await _identityRepository.CreateRoleAsync(new Role { Name = role });
                }
                if (dbRecordRole != null)
                    await _identityRepository.AddRoleToUserAsync(user, role);
            }

            return true;
        }

        public async Task<List<User>> GetUsers()
        {
            var DbRecords = await _identityRepository.GetUsersAsync();
            List<User> users = new();
            foreach (var user in DbRecords)
            {
                var dbRecordNameRole = await _identityRepository.GetRolesByUserIdAsync(user.Id);
                List<Role> roles = new();
                foreach (var roleName in dbRecordNameRole)
                {
                    var role = await _identityRepository.GetRoleByNameAsync(roleName);
                    roles.Add(role);
                }
                user.Roles = roles;
                users.Add(user);
            }
            return users;
        }

        public async Task<bool> DeleteUser(string id) =>
            await _identityRepository.RemoveUserByIdAsync(id);

        public async Task<bool> UpdateUser(User input) =>
            await _identityRepository.UpdateUserAsync(input);

        public async Task<User> GetUserByUserClaim(ClaimsPrincipal userClaim) =>
            await _identityRepository.GetUserByUserClaim(userClaim);

        public async Task<bool> ChangePassword(User user, string currentPassword, string newPassword) =>
            await _identityRepository.ChangePasssword(user, currentPassword, newPassword);

        public async Task<bool> CheckPassword(User user, string Password) =>
            await _identityRepository.CheckPassword(user, Password);

        public async Task<bool> CreateRoleAsync(Role inputRole) =>
            await _identityRepository.CreateRoleAsync(inputRole);

        public async Task<bool> UpdateRoleAsync(Role inputRole) =>
            await _identityRepository.UpdateRoleAsync(inputRole);

        public async Task<bool> DeleteRoleByIdAsync(string id) =>
            await _identityRepository.DeleteRoleByIdAsync(id);

        public async Task<bool> DeleteRoleByNameAsync(string name) =>
            await _identityRepository.DeleteRoleByNameAsync(name);

        public async Task<bool> AddRoleToUserAsync(User user, string role) =>
            await _identityRepository.AddRoleToUserAsync(user, role);

        public async Task<bool> DeleteRoleInUser(User user, string role) =>
            await _identityRepository.DeleteRoleInUser(user, role);

        public async Task<Role> GetRoleByIdAsync(string id) =>
            await _identityRepository.GetRoleByIdAsync(id);
        public async Task<Role> GetRoleByNameAsync(string name) =>
            await _identityRepository.GetRoleByNameAsync(name);

        public async Task<PagedList<Role>> GetRolesAsync(RoleParams roleParams)
        {
            var roles = _identityRepository.GetRolesQuery();
            return await PagedList<Role>.CreatePagingListAsync(roles, roleParams.PageNumber, roleParams.PageSize);
        }

        public async Task<PagedList<ReportMessage>> ShowReportedMessages(ReportMessageParams reportParams)
        {
            var messages = _reportMessageRepository.GetQuery().Include(us => us.UserSendReport).Include(x => x.Message).ThenInclude(x => x.Sender);
            return await PagedList<ReportMessage>.CreatePagingListAsync(messages , reportParams.PageNumber , reportParams.PageSize);
        }

        public async Task<List<ReportMessage>> ShowReportedMessage(int id) =>
            await _reportMessageRepository.GetQuery().Where(x => x.MessageId == id).Include(us => us.UserSendReport).Include(x => x.Message).ThenInclude(x => x.Sender).ToListAsync();

        public async Task<List<ReportComment>> ShowReportedComments() =>
            await _reportCommentRepository.GetQuery().Include(us => us.UserSendReport).Include(x => x.Comment).ToListAsync();

        public async Task<List<ReportComment>> ShowReportedComment(int id) =>
            await _reportCommentRepository.GetQuery().Where(x => x.CommentId == id).Include(us => us.UserSendReport).Include(x => x.Comment).ToListAsync();

        public async Task<List<ReportCourse>> ShowReportedCourses() =>
            await _reportCourseRepository.GetQuery().Include(x => x.Course).ThenInclude(x => x.Teacher).ThenInclude(u => u.User).ToListAsync();

        public async Task<List<ReportCourse>> ShowReportedCourse(int id) =>
            await _reportCourseRepository.GetQuery().Where(x => x.CourseId == id).Include(x => x.Course).ThenInclude(x => x.Teacher).ThenInclude(u => u.User).ToListAsync();

        public async Task<List<ReportUser>> ShowReportedUsers() =>
            await _reportUserRepository.GetQuery().Include(x => x.UserSendReport).Include(x => x.UserReciveReport).ToListAsync();

        public async Task<List<ReportUser>> ShowReportedUser(string id) =>
            await _reportUserRepository.GetQuery().Where(x => x.UserReciveReportId == id).Include(x => x.UserSendReport).Include(x => x.UserReciveReport).ToListAsync();

        public async Task<bool> DeleteMessage(int messageId) =>
            await _messageRepository.DeleteAsync(messageId);

        public async Task<bool> DeleteComment(int commentId) =>
            await _commentRepository.DeleteAsync(commentId);

        public async Task<bool> DeleteSubComment(int subCommentId) =>
            await _subCommentRepository.DeleteAsync(subCommentId);
        public async Task<bool> DeleteCourse(int courseId) =>
            await _courseRepository.DeleteAsync(courseId);
    }
    public interface IDashboardService
    {
        public Task<bool> RegisterUser(UserRegister input);
        public Task<User> GetUserByIdAsync(string id);
        public Task<User> GetUserByNameAsync(string name);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<List<User>> GetUsers();
        public Task<bool> UpdateUser(User input);
        public Task<bool> DeleteUser(string id);
        public Task<User> GetUserByUserClaim(ClaimsPrincipal userClaim);
        public Task<bool> ChangePassword(User user, string currentPassword, string newPassword);
        public Task<bool> CheckPassword(User user, string Password);
        public Task<bool> CreateRoleAsync(Role inputRole);
        public Task<bool> UpdateRoleAsync(Role inputRole);
        public Task<bool> DeleteRoleByIdAsync(string id);
        public Task<bool> DeleteRoleByNameAsync(string name);
        public Task<List<string>> GetRolesByUserIdAsync(string id);
        public Task<bool> AddRoleToUserAsync(User user, string role);
        public Task<bool> DeleteRoleInUser(User user, string role);
        public Task<Role> GetRoleByIdAsync(string id);
        public Task<Role> GetRoleByNameAsync(string name);
        public Task<PagedList<Role>> GetRolesAsync(RoleParams roleParams);
        public Task<PagedList<ReportMessage>> ShowReportedMessages(ReportMessageParams reportParams);
        public Task<List<ReportMessage>> ShowReportedMessage(int id);
        public Task<List<ReportComment>> ShowReportedComments();
        public Task<List<ReportComment>> ShowReportedComment(int id);
        public Task<List<ReportCourse>> ShowReportedCourses();
        public Task<List<ReportCourse>> ShowReportedCourse(int id);
        public Task<List<ReportUser>> ShowReportedUsers();
        public Task<List<ReportUser>> ShowReportedUser(string id);
        public Task<bool> DeleteMessage(int messageId);
        public Task<bool> DeleteComment(int commentId);
        public Task<bool> DeleteSubComment(int subCommentId);
        public Task<bool> DeleteCourse(int courseId);
    }
}