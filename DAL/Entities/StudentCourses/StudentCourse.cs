using System;
using DAL.Entities.Common;
using DAL.Entities.Courses;
using DAL.Entities.Identity;

namespace DAL.Entities.StudentCourses
{
    public class StudentCourse : BaseEntity
    {
        public DateTime RegistDate { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual Course Course { get; set; }
        public int CourseId { get; set; }
    }
}