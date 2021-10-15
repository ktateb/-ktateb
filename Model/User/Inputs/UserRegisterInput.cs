using System;
using FluentValidation;

namespace Model.User.Inputs
{
    public class UserRegisterInput
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
    }
    public class UserRegisterInputValidator : AbstractValidator<UserRegisterInput>
    {
        public UserRegisterInputValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Please enter your username");
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Please enter your FirstName");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Please enter your LastName");
            RuleFor(x => x.Birthday)
                .NotEmpty().WithMessage("Please enter your Birthday");
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Please enter your Country");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Please enter your Phone");
            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please enter your Email")
                .EmailAddress().WithMessage("This email is not valid");

            RuleFor(x => x.PhoneNumber)
                .Must(x => long.TryParse(x, out var val) && val > 0).WithMessage("Invalid Number.");

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