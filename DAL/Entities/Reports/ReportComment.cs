using System;
using System.Collections.Generic;
using DAL.Entities.Comments;
using DAL.Entities.Common;
using DAL.Entities.Identity;

namespace DAL.Entities.Reports
{
    public class ReportComment : BaseEntity
    {
        public string Text { get; set; }
        public DateTime DateReport { get; set; }
        public virtual User UserSendReport { get; set; }
        public string UserId { get; set; }
        public virtual Comment Comment { get; set; }
        public int CommentId { get; set; }
    }
}