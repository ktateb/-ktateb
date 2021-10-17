using System;
using FluentValidation;

namespace Model.User.Inputs
{
    public class UserUpdate
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
    }
    public class UserUpdateValidator : AbstractValidator<UserUpdate>
    {
        public UserUpdateValidator()
        {

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

            RuleFor(x => x.PhoneNumber)
                .Must(x => long.TryParse(x, out var val) && val > 0).WithMessage("Invalid Number.");
        }
    }
}