using System;

namespace Model.Comment.Inputs
{
    public class CommentCreateInput
    {
        public int CourseId { get; set; }
        public int CommentText { get; set; }
        public DateTime DateComment{get;set;}
    }
}