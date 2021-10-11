using DAL.Entities.Common;
using DAL.Entities.Identity;

namespace DAL.Entities.Students
{
    public class Student : BaseEntity
    {
        public virtual User User { get; set; }
        public string UserId { get; set; }
        // MyCourse
        // My Favorite Course
    }
}