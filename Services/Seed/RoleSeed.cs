using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Services.Seed
{
    public class RoleSeed
    {
        public static async Task SeedRoleAsync(RoleManager<Role> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                List<Role> roles = new()
                {
                    new Role() { Name = "Admin" },
                    new Role() { Name = "Manager" },
                    new Role() { Name = "Student" },
                    new Role() { Name = "Teacher" }
                };
                foreach (var role in roles)
                    await roleManager.CreateAsync(role);
            }
        }
    }
}