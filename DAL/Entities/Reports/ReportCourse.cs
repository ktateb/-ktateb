using System;
using DAL.Entities.Courses;
using DAL.Entities.Identity;

namespace DAL.Entities.Reports
{
    public class ReportCourse
    {
        public string Text { get; set; }
        public DateTime DateReport { get; set; }
        public virtual User UserSendReport { get; set; }
        public string UserId { get; set; }
        public virtual Course Course { get; set; }
        public int CourseId { get; set; }
    }
}