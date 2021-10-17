using System;
using System.Collections.Generic;
using DAL.Entities.Common;
using DAL.Entities.Identity;
using DAL.Entities.Reports;

namespace DAL.Entities.Comments
{
    public class SubComment : BaseEntity
    {
        public string Text { get; set; }
        public DateTime DateComment { get; set; }
        public bool IsUpdated { get; set; }
        public virtual Comment Comment { get; set; }
        public int CommentId { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<ReportComment> ReportsComment { get; set; }
    }
}