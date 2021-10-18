using System;

namespace Model.Report.User.Outputs
{
    public class ShowReportUserForDashboard
    {
        public string ReportText { get; set; }
        public DateTime DateReport { get; set; }
        public string UserSendReport { get; set; }
        public string UserReciveReport { get; set; }
    }
}