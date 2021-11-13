using System.Collections.Generic;

namespace Model.User.Outputs
{
    public class UsernameAndRolesOnly
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}