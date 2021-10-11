using DAL.Entities.Common;
using System;
namespace DAL.Entities.StudentFavoriteCourse
{
    public class StudentFavoriteCourse:BaseEntity
    {
        public DateTime AddedDate { get; set; }

        public virtual Students.Student Student { get; set; }

        public int StudentId { get; set; }

        public virtual Courses.Course Course { get; set; }

        public int CourseId { get; set; }
    }
}