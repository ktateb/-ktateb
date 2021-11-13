using System.Collections.Generic;
using System;
using DAL.Entities.Common;
using DAL.Entities.StudentFavoriteCourses;
using DAL.Entities.Reports;
using DAL.Entities.Comments;
using DAL.Entities.Ratings;
using DAL.Entities.StudentCourses;
using DAL.Entities.Categories;
using DAL.Entities.Teachers;

namespace DAL.Entities.Courses
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string OverViewDescription { get; set; }
        public string LearnListDescription { get; set; }
        public string ThisCourseFor { get; set; }
        public string CourseRequerment { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public virtual ICollection<CourseSection> CourseSections { get; set; }
        public virtual ICollection<StudentCourse> Students { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual ICollection<ReportCourse> ReportList { get; set; }
        public virtual ICollection<StudentFavoriteCourse> FavoriteByList { get; set; }
    }
}