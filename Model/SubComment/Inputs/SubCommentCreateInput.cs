using System;
using System.ComponentModel.DataAnnotations; 
using FluentValidation;
namespace Model.SubComment.Inputs
{
    public class SubCommentCreateInput
    {
        public int BaseCommentId { get; set; }
        [Required]
        public string CommentText { get; set; }
    }
    public class SubCommentCreateInputValidator : AbstractValidator<SubCommentCreateInput>
    {
        public SubCommentCreateInputValidator()
        {
            RuleFor(x => x.CommentText)
                .NotEmpty().WithMessage("Please enter the  Comment");
        }
    }

}