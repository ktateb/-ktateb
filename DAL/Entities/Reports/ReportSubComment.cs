using System;
using DAL.Entities.Comments;
using DAL.Entities.Common;
using DAL.Entities.Identity;

namespace DAL.Entities.Reports
{
    public class ReportSubComment : BaseEntity
    {
        public string Text { get; set; }
        public DateTime DateReport { get; set; }
        public virtual User UserSendReport { get; set; }
        public string UserId { get; set; }
        public virtual SubComment SubComment { get; set; }
        public int SubCommentId { get; set; }
    }
}