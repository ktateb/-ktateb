namespace Model.Message.Inputs
{
    public class MessageParam
    {
        public string UserReciverId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? SenderDeleted { get; set; }
        public bool? RecipentDeleted { get; set; }
    }
}