using System;

namespace Model.Message.Outputs
{
    public class MessageOutput
    {
        public int Id { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime DateRead { get; set; }
        public bool IsRead { get; set; }
        public bool SenderDeleted { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; } 
        public bool RecipentDeleted { get; set; }
        public string Text { get; set; }
        public string SenderId { get; set; }
        public string ReciverId { get; set; }
    }
}