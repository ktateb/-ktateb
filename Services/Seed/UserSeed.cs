using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.DataContext;
using DAL.Entities.Identity;
using DAL.Entities.Identity.enums;
using Microsoft.AspNetCore.Identity;
using Model.User.Outputs;
using Services.Seed.GenerateDummyData;

namespace Services.Seed
{
    public class UserSeed
    {
        public static async Task SeedUserAsync(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var usersData = await File.ReadAllTextAsync("../Services/Seed/Data/Users.json");
                var users = JsonSerializer.Deserialize<List<UserSeedOutput>>(usersData);
                foreach (var user in users)
                {
                    var userForAdd = new User
                    {
                        Birthday = DateTime.UtcNow,
                        Gender = Gender.Male,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Country = user.Country,
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(userForAdd, user.Password);
                    string UserName = user.UserName;
                    var dbRecord = await userManager.FindByNameAsync(UserName);
                    await userManager.AddToRoleAsync(dbRecord, "Admin");
                }

                var generatedUsers = GenerateUsers.AddUsers();
                foreach (var user in generatedUsers)
                {
                    await userManager.CreateAsync(user, "123@Abc");
                    string UserName = user.UserName;
                    var dbRecord = await userManager.FindByNameAsync(UserName);
                    await userManager.AddToRoleAsync(dbRecord, "Student");
                }
            }
        }
    }
}