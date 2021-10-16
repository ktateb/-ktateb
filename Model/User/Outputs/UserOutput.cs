using System.Collections.Generic;
using DAL.Entities.Identity;

namespace Model.User.Outputs
{
    public class UserOutput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public string DisplayName { get; set; }
        public List<string> Roles { get; set; }

    }
}