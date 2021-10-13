using System.Collections.Generic;
using DAL.Entities.Common;
using DAL.Entities.StudentCourses;
using DAL.Entities.Identity;
using DAL.Entities.StudentFavoriteCourses;
using DAL.Entities.CourseQuizes;
using DAL.Entities.StudentWatches;
namespace DAL.Entities.Students
{
    public class Student : BaseEntity
    {
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<StudentCourse> Courses { get; set; }
        public virtual ICollection<StudentFavoriteCourse> FavorateCourses { get; set; }
        public virtual ICollection<StudentAnswer> QuizzesAnswers  { get; set; } 
        public virtual ICollection<StudentWatchedVedio> WatchedVedios  { get; set; }
    }
}