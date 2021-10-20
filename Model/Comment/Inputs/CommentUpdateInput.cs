using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Model.Comment.Inputs
{
    public class CommentUpdateInput
    {
        public int Id { get; set; }
        [Required]
        public string CommentText { get; set; }
    }
    public class CommentUpdateInputValidator : AbstractValidator<CommentUpdateInput>
    {
        public CommentUpdateInputValidator()
        {
            RuleFor(x => x.CommentText)
                .NotEmpty().WithMessage("Please enter the  Comment");
        }
    }
}