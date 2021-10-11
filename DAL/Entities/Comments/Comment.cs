using System;
using System.Collections.Generic;
using DAL.Entities.Common;
using DAL.Entities.Identity;
using DAL.Entities.Reports;

namespace DAL.Entities.Comments
{
    public class Comment : BaseEntity
    {
        public DateTime DateComment { get; set; }
        public string Text { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<ReportComment> ReportsComment { get; set; }
    }
}