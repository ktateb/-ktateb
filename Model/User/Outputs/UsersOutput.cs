using System.Collections.Generic;

namespace Model.User.Outputs
{
    public class UsersOutput
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string DisplayName { get; set; }
        public List<string> Roles { get; set; }
    }
}