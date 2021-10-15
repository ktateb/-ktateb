using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities.Countries;
using DAL.Entities.Identity;
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

        public async Task<User> GetUserByEmailAsync(string email) =>
            await _identityRepository.GetUserByEmailAsync(email);

        public async Task<User> GetUserByIdAsync(string id) =>
            await _identityRepository.GetUserByIdAsync(id);

        public async Task<User> GetUserByNameAsync(string name) =>
            await _identityRepository.GetUserByNameAsync(name);

        public async Task<bool> LoginUser(User user, string password) =>
            await _identityRepository.LoginUser(user, password);

        public async Task<List<string>> GetRolesByUserIdAsync(string id) =>
            await _identityRepository.GetRolesByUserIdAsync(id);

        public async Task<bool> RegisterUser(UserRegisterInput input)
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
                Country = input.Country
            };

            var result = await _identityRepository.CreateUserAsync(user, input.Password);
            if (!result)
                return false;
            return true;
        }

        public async Task<List<User>> GetUsers() =>
            await _identityRepository.GetUsersAsync();

        public async Task<bool> DeleteUser(string id) =>
            await _identityRepository.RemoveUserByIdAsync(id);

        public async Task<bool> UpdateUser(User input)
        {
            var dbRecord = await _identityRepository.GetUserByIdAsync(input.Id);
            dbRecord.FirstName =input.FirstName;
            dbRecord.LastName =input.LastName;
            dbRecord.PictureUrl =input.PictureUrl;
            dbRecord.Country =input.Country;
            dbRecord.PhoneNumber =input.PhoneNumber;
            return await _identityRepository.UpdateUserAsync(dbRecord);
        }
    }
    public interface IAccountService
    {
        public Task<bool> LoginUser(User user, string password);
        public Task<bool> RegisterUser(UserRegisterInput input);
        public Task<User> GetUserByIdAsync(string id);
        public Task<User> GetUserByNameAsync(string name);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<List<User>> GetUsers();
        public Task<bool> UpdateUser(User input);
        public Task<bool> DeleteUser(string id);
        public Task<List<string>> GetRolesByUserIdAsync(string id);
    }
}