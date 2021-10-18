using System;
using FluentValidation;

namespace Model.Message.Inputs
{
    public class MessageInput
    {
        public string Text { get; set; }
        public string ReciverId { get; set; }
    }

    public class MessageInputValidator : AbstractValidator<MessageInput>
    {
        public MessageInputValidator()
        {
            RuleFor(r => r.Text).NotNull().WithMessage("The message must be not null");
            RuleFor(r => r.ReciverId).NotNull().WithMessage("Please enter ReciveId");
        }
    }
}