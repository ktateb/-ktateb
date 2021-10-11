using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DataContext;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly StoreContext _dbContext;
        public IdentityRepository(StoreContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateUserAsync(User inputUser, string password)
        {
            var dbRecord = await _userManager.FindByIdAsync(inputUser.Id);
            if (dbRecord != null)
                return false;
            await _userManager.CreateAsync(inputUser, password);
            return true;
        }

        public async Task<User> GetUserByIdAsync(string id) =>
            await _userManager.FindByIdAsync(id);

        public async Task<User> GetUserByNameAsync(string name) =>
            await _userManager.FindByNameAsync(name);

        public async Task<List<User>> GetUsersAsync() =>
            await _dbContext.Users.ToListAsync();

        public async Task<bool> RemoveUserByNameAsync(string name)
        {
            var dbRecord = await _userManager.FindByNameAsync(name);
            if (dbRecord == null)
                return false;
            await _userManager.DeleteAsync(dbRecord);
            return true;
        }

        public async Task<bool> RemoveUserByIdAsync(string id)
        {
            var dbRecord = await _userManager.FindByIdAsync(id);
            if (dbRecord == null)
                return false;
            await _userManager.DeleteAsync(dbRecord);
            return true;
        }

        public async Task<bool> UpdateUserAsync(User inputUser)
        {
            var dbRecord = await _userManager.FindByNameAsync(inputUser.UserName);
            if (dbRecord == null)
                return false;
            await _userManager.UpdateAsync(dbRecord);
            return true;
        }

        public async Task<Role> GetRoleByIdAsync(string id) =>
            await _roleManager.FindByIdAsync(id);

        public async Task<Role> GetRoleByNameAsync(string name) =>
            await _roleManager.FindByNameAsync(name);

        public async Task<List<IdentityRole>> GetRolesAsync() =>
            await _dbContext.Roles.ToListAsync();

        public async Task<bool> CreateRoleAsync(Role inputRole)
        {
            var dbRecord = await _roleManager.FindByNameAsync(inputRole.Name);
            if (dbRecord != null)
                return false;
            await _roleManager.CreateAsync(inputRole);
            return true;
        }

        public async Task<bool> UpdateRoleAsync(Role inputRole)
        {
            var dbRecord = await _roleManager.FindByNameAsync(inputRole.Name);
            if (dbRecord == null)
                return false;
            await _roleManager.UpdateAsync(inputRole);
            return true;
        }

        public async Task<bool> DeleteRoleByIdAsync(string id)
        {
            var dbRecord = await _roleManager.FindByIdAsync(id);
            if (dbRecord == null)
                return false;
            await _roleManager.DeleteAsync(dbRecord);
            return true;
        }

        public async Task<bool> DeleteRoleByNameAsync(string name)
        {
            var dbRecord = await _roleManager.FindByNameAsync(name);
            if (dbRecord == null)
                return false;
            await _roleManager.DeleteAsync(dbRecord);
            return true;
        }
    }
    public interface IIdentityRepository
    {
        public Task<bool> CreateUserAsync(User inputUser, string password);
        public Task<bool> RemoveUserByNameAsync(string name);
        public Task<bool> RemoveUserByIdAsync(string id);
        public Task<bool> UpdateUserAsync(User inputUser);
        public Task<User> GetUserByIdAsync(string id);
        public Task<User> GetUserByNameAsync(string name);
        public Task<List<User>> GetUsersAsync();
        public Task<Role> GetRoleByIdAsync(string id);
        public Task<Role> GetRoleByNameAsync(string name);
        public Task<List<IdentityRole>> GetRolesAsync();
        public Task<bool> CreateRoleAsync(Role inputRole);
        public Task<bool> UpdateRoleAsync(Role inputRole);
        public Task<bool> DeleteRoleByIdAsync(string id);
        public Task<bool> DeleteRoleByNameAsync(string id);
    }
}