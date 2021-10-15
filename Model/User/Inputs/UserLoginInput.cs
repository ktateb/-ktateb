using FluentValidation;

namespace Model.User.Inputs
{
    public class UserLoginInput
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
    public class UserLoginInputValidator : AbstractValidator<UserLoginInput>
    {
        public UserLoginInputValidator()
        {
            RuleFor(x =>x.UserNameOrEmail)
                .NotEmpty().WithMessage("Please enter your UserName Or Email");            
            RuleFor(x =>x.Password)
                .NotEmpty().WithMessage("Please enter your passwrod");            
        }
    }
}