using DAL.Entities.Comments;
using DAL.Entities.Countries;
using DAL.Entities.CourseQuizes;
using DAL.Entities.Messages;
using DAL.Entities.Ratings;
using DAL.Entities.Reports;
using DAL.Entities.StudentCourses;
using DAL.Entities.StudentFavoriteCourses;
using DAL.Entities.StudentWatches;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
        public string PictureUrl { get; set; }
        public DateTime StartRegisterDate { get; set; }
        public virtual ICollection<Message> UserSenders { get; set; }
        public virtual ICollection<Message> UserRecivers { get; set; }
        public virtual ICollection<ReportUser> UsersSendReportToUser { get; set; }
        public virtual ICollection<ReportUser> UsersReciveReportFromUser { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<SubComment> SubComments { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<StudentCourse> Courses { get; set; }
        public virtual ICollection<StudentFavoriteCourse> FavorateCourses { get; set; }
        public virtual ICollection<StudentAnswer> QuizzesAnswers  { get; set; } 
        public virtual ICollection<StudentWatchedVedio> WatchedVedios  { get; set; }

        [NotMapped]
        public string FullName { get => FirstName + " " + LastName; }
    }
}