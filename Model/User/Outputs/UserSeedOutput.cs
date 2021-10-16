using System.Collections.Generic;
using DAL.Entities.Identity;

namespace Model.User.Outputs
{
    public class UserSeedOutput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public bool EmailConfirmd { get; set; }
    }
}