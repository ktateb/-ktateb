using DAL.Entities.Common;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using System.Collections.Generic;
namespace DAL.Entities.Teachers
{
    public class Teacher : BaseEntity
    {
        public string AboutMe { get; set; }
        public string LinkedInUrl { get; set; }
        public string WhatsappUrl { get; set; }
        public string FaceBookUrl { get; set; }
        public string TelegramUrl { get; set; }
        public string Specialist { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}