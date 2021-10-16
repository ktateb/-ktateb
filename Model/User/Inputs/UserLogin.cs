using FluentValidation;

namespace Model.User.Inputs
{
    public class UserLogin
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {
            RuleFor(x =>x.UserNameOrEmail)
                .NotEmpty().WithMessage("Please enter your UserName Or Email");            
            RuleFor(x =>x.Password)
                .NotEmpty().WithMessage("Please enter your passwrod");            
        }
    }
}