using System;

namespace Model.Report.Comment.Outputs
{
    public class ReportCommentForDashboard
    {
        public string ReportText { get; set; }
        public DateTime DateReport { get; set; }
        public string UserName { get; set; }
        public string CommentText { get; set; }
        public DateTime DateSentComment { get; set; }
        public string UserNameSentComment { get; set; }
    }
}