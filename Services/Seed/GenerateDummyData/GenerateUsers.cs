using System.Collections.Generic;
using DAL.Entities.Identity;
using DAL.Entities.Identity.enums;

namespace Services.Seed.GenerateDummyData
{
    public class GenerateUsers
    {

        private const int counter = 100;
        public static List<User> AddUsers()
        {
            List<User> users = new();
            for (int i = 0; i < counter; i++)
            {
                User user = new()
                {
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last(),
                    Birthday = System.DateTime.UtcNow,
                    Country = Faker.Country.Name(),
                    Gender = Gender.Male,
                    Email = "User" + (i + 1) + "@Ktateb.com",
                    UserName = "User" + (i + 1)
                };
                users.Add(user);
            }
            return users;
        }
    }
}