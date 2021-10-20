using System;
namespace Model.SubComment.Outputs
{
    public class SubCommentOutput
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public string UserDisplayName { get; set; }
        public string UserName { get; set; }
        public string UserPictureUrl { get; set; }
        public bool IsEdited { get; set; }
        public DateTime Date{get;set;}
        
    }
}