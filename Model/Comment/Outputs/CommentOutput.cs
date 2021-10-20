using System;
namespace Model.Comment.Outputs
{
    public class CommentOutput
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public string UserDisplayName { get; set; }
        public string UserName { get; set; }
        public string UserPictureUrl { get; set; } 
        public bool IsEdited { get; set; }
        public DateTime Date { get; set; } 
    } 
}