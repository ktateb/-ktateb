using System.Collections.Generic;
using System;
using DAL.Entities.Common;
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

        public virtual Teachers.Teacher Teacher { get; set; }

        public int TeacherId { get; set; }

        public virtual Categories.Category Category { get; set; }

        public int CategoryId { get; set; }

        public virtual ICollection<CourseSection> CourseSections { get; set; }

        public virtual ICollection<StudentCourses.StudentCourse> Students { get; set; }

        public virtual ICollection<Ratings.Rating> Ratings { get; set; }

        public virtual ICollection<Comments.Comment> Comments { get; set; }

        public virtual ICollection<Reports.ReportCourse> ReportList { get; set; }

        public virtual ICollection<StudentFavoriteCourse.StudentFavoriteCourse> FavoriteByList { get; set; }



    }
}