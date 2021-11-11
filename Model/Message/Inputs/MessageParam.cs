using Model.Helper;

namespace Model.Message.Inputs
{
    public class MessageParam : Paging
    {
        public string UserReciverId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? SenderDeleted { get; set; }
        public bool? RecipentDeleted { get; set; }
    }
}