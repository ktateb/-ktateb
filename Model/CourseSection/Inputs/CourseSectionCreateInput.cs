using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Model.CourseSection.Inputs
{
    public class CourseSectionCreateInput
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int CourseId { get; set; }
    }
    public class CourseSectionCreateInputValidator : AbstractValidator<CourseSectionCreateInput>
    {
        public CourseSectionCreateInputValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Please enter the  Course title");
            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("Please put Description for this Course");
        }
    }
}