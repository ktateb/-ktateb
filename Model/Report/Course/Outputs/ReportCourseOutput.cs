using System;

namespace Model.Report.Course.Outputs
{
    public class ReportCourseOutput
    {
        public string Text { get; set; }
        public DateTime DateReport { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
    }
}