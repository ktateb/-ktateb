using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Model.SubComment.Inputs
{
    
        public class SubCommentUpdateInput
        {
            public int Id { get; set; }
            [Required]
            public string CommentText { get; set; }
        }
        public class SubCommentUpdateInputValidator : AbstractValidator<SubCommentUpdateInput>
        {
            public SubCommentUpdateInputValidator()
            {
                RuleFor(x => x.CommentText)
                    .NotEmpty().WithMessage("Please enter the  Comment");
            }
        }
    
}