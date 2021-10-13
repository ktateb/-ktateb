using System.Collections.Generic;
using DAL.Entities.Identity;

namespace Model.User.Outputs
{
    public class UserOutput
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string DisplayName { get; set; }
        public List<string> Roles { get; set; }

    }
}