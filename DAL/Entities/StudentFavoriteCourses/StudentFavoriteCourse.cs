using DAL.Entities.Common;
using DAL.Entities.Courses;
using DAL.Entities.Students;
using System;
namespace DAL.Entities.StudentFavoriteCourses
{
    public class StudentFavoriteCourse : BaseEntity
    {
        public DateTime AddedDate { get; set; }
        public virtual Student Student { get; set; }
        public int StudentId { get; set; }
        public virtual Course Course { get; set; }
        public int CourseId { get; set; }
    }
}