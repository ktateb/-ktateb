using System;

namespace Model.Report.Message.Outputs
{
    public class ReportMessageForDashboard
    {
        public string ReportText { get; set; }
        public DateTime DateReport { get; set; }
        public string UserName { get; set; }
        public string MessageText { get; set; }
        public DateTime DateSentMessage { get; set; }
        public string UserNameSentThisMessage { get; set; }

    }
}