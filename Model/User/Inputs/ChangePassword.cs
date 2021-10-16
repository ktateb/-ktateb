using FluentValidation;

namespace Model.User.Inputs
{
    public class ChangePassword
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }

    }
    public class ChangePasswordValidator : AbstractValidator<ChangePassword>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Please enter your Current password");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Please enter your password")
                .Length(8, 20).WithMessage("Paswword must have minimum length 8 and maximum length 20")
                .Matches("[A-Z]").WithMessage("Password must have uppercase letter")
                .Matches("[a-z]").WithMessage("Password must have lowercase letter")
                .Matches("[0-9]").WithMessage("Password must have digit")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must have special character");
        }
    }
}