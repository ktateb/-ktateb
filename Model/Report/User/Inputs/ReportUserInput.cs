using FluentValidation;

namespace Model.Report.User.Inputs
{
    public class ReportUserInput
    {
        public string Text { get; set; }
        public string UserReciveReportId { get; set; }
    }
    public class ReportUserInputValidator : AbstractValidator<ReportUserInput>
    {
        public ReportUserInputValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Please enter why you report this user");
            RuleFor(x => x.UserReciveReportId)
                .NotEmpty().WithMessage("Please enter the user you will report it");
        }
    }
}