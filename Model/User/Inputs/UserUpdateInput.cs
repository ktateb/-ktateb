using System;

namespace Model.User.Inputs
{
    public class UserUpdateInput
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
        public string PictureUrl { get; set; }
    }
}