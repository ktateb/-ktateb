using System;

namespace Model.Report.Comment.Outputs
{
    public class ReportCommentOutput
    {
        public string Text { get; set; }
        public string UserId { get; set; }
        public int CommentId { get; set; }
        public DateTime DateReported { get; set; }
    }
}