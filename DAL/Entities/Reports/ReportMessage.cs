using System;
using DAL.Entities.Identity;
using DAL.Entities.Messages;

namespace DAL.Entities.Reports
{
    public class ReportMessage
    {
        public string Text { get; set; }
        public DateTime DateReport { get; set; }
        public virtual User UserSendReport { get; set; }
        public string UserId { get; set; }
        public virtual Message Message { get; set; }
        public int MessageId { get; set; }
    }
}