using DAL.Entities.Common;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using DAL.Entities.Ratings.enums;

namespace DAL.Entities.Ratings
{
    public class Rating : BaseEntity
    {
        public RatingStar RatingStar { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual Course Course { get; set; }
        public int CourseId { get; set; }
    }
}