using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.Entities.Countries;
using DAL.Entities.Identity;
using DAL.Entities.Identity.enums;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.User.Inputs;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IIdentityRepository _identityRepository;

        public AccountService(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
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

        public async Task<bool> LoginUser(User user, string password) =>
            await _identityRepository.LoginUser(user, password);

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


    }
    public interface IAccountService
    {
        public Task<bool> LoginUser(User user, string password);
        public Task<bool> RegisterUser(UserRegister input);
        public Task<User> GetUserByIdAsync(string id);
        public Task<User> GetUserByNameAsync(string name);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<List<User>> GetUsers();
        public Task<bool> UpdateUser(User input);
        public Task<bool> DeleteUser(string id);
        public Task<List<string>> GetRolesByUserIdAsync(string id);
        public Task<User> GetUserByUserClaim(ClaimsPrincipal userClaim);
        public Task<bool> ChangePassword(User user, string currentPassword, string newPassword);
        public Task<bool> CheckPassword(User user, string Password);

    }
}