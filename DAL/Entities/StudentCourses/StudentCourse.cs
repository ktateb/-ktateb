using System;

using DAL.Entities.Common;
namespace DAL.Entities.StudentCourses
{
    public class StudentCourse:BaseEntity
    {

        public DateTime RegistDate { get; set; }
        
        public virtual Students.Student Student { get; set; }
        
        public int StudentId { get; set; }

        public virtual Courses.Course Course { get; set; }
        
        public int CourseId { get; set; }
    }
}