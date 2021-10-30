using DAL.Entities.Common;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using System;
namespace DAL.Entities.StudentFavoriteCourses
{
    public class StudentFavoriteCourse : BaseEntity
    {
        public DateTime AddedDate { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual Course Course { get; set; }
        public int CourseId { get; set; }
    }
}