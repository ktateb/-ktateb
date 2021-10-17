namespace Model.Report.Message.Outputs
{
    public class ReportMessageOutput
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public System.DateTime DateReport { get; set; }
        public string UserId { get; set; }
        public int MessageId { get; set; }
    }
}