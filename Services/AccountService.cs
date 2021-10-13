using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities.Identity;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Model.User.Inputs;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IdentityRepository _identityRepository;

        public AccountService(IdentityRepository identityRepository)
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

    }
    public interface IAccountService
    {
        public Task<bool> LoginUser(User user, string password);
        public Task<User> GetUserByIdAsync(string id);
        public Task<User> GetUserByNameAsync(string name);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<List<string>> GetRolesByUserIdAsync(string id);
    }
}