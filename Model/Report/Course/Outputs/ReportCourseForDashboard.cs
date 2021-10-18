using System;

namespace Model.Report.Course.Outputs
{
    public class ReportCourseForDashboard
    {
        public string ReportText { get; set; }
        public DateTime DateReport { get; set; }
        public string UserName { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
    }
}