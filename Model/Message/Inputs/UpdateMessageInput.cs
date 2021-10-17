using FluentValidation;

namespace Model.Message.Inputs
{
    public class UpdateMessageInput
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ReciverId { get; set; }
    }
    public class UpdateMessageInputValidator : AbstractValidator<UpdateMessageInput>
    {
        public UpdateMessageInputValidator()
        {
            RuleFor(r => r.Text).NotNull().WithMessage("The message must be not null");
            RuleFor(r => r.ReciverId).NotNull().WithMessage("Please enter ReciveId");
        }
    }
}