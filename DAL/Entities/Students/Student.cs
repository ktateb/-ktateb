using System.Collections.Generic;
using DAL.Entities.Common;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using DAL.Entities.StudentFavoriteCourses;

namespace DAL.Entities.Students
{
    public class Student : BaseEntity
    {
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<StudentFavoriteCourse> FavorateCourses { get; set; }
    }
}