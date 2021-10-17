using System;

namespace Model.Report.User.Outputs
{
    public class ReportUserOutput
    {
        public string Text { get; set; }
        public DateTime DateReport { get; set; }
        public string UserSendReportId { get; set; }
        public string UserReciveReportId { get; set; }
    }
}