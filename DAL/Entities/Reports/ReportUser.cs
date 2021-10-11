using System;
using DAL.Entities.Common;
using DAL.Entities.Identity;

namespace DAL.Entities.Reports
{
    public class ReportUser : BaseEntity
    {
        public string Text { get; set; }
        public DateTime DateReport { get; set; }
        public virtual User UserSendReport { get; set; }
        public string UserSendReportId { get; set; }
        public virtual User UserReciveReport { get; set; }
        public string UserReciveReportId { get; set; }
    }
}