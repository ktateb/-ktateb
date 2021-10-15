using System.Collections.Generic;

namespace Model.User.Outputs
{
    public class UsersOutput
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public List<string> Roles { get; set; }
    }
}