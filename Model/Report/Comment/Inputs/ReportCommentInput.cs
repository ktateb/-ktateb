using FluentValidation;

namespace Model.Report.Comment.Inputs
{
    public class ReportCommentInput
    {
        public string Text { get; set; }
        public int CommentId { get; set; }
    }
    public class ReportCommentInputValidator : AbstractValidator<ReportCommentInput>
    {
        public ReportCommentInputValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Please enter why you report this comment");
            RuleFor(x => x.CommentId)
                .NotEmpty().WithMessage("Please enter the comment you will report it");
        }
    }
}