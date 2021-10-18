using DAL.Entities.Ratings.enums;
using FluentValidation;

namespace Model.Rating.Inputs
{
    public class RatingInput
    {
        public int RatingStar { get; set; }
        public int CourseId { get; set; }
    }
    public class RatingInputValidator : AbstractValidator<RatingInput>
    {
        public RatingInputValidator()
        {
            RuleFor(x => x.RatingStar)
                .NotNull().WithMessage("Please enter ratings")
                .InclusiveBetween(1, 5).WithMessage("Please enter ratings between 1 - 5");

            RuleFor(x => x.CourseId)
                .NotNull().WithMessage("Please enter course your need rating it");
        }
    }
}