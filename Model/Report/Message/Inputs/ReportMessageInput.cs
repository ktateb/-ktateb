using System;
using FluentValidation;

namespace Model.Report.Message.Inputs
{
    public class ReportMessageInput
    {
        public string Text { get; set; }
        public int MessageId { get; set; }
    }
    public class ReportMessageInputValidator : AbstractValidator<ReportMessageInput>
    {
        public ReportMessageInputValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Please enter why you report this message");
            RuleFor(x => x.MessageId)
                .NotEmpty().WithMessage("Please enter the message you will report it");
        }
    }
}