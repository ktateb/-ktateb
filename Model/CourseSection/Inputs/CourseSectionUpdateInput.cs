using FluentValidation;
using System.ComponentModel.DataAnnotations;
namespace Model.CourseSection.Inputs
{
    public class CourseSectionUpdateInput
    {
        
        public int SectionId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; } 
    }
    public class CourseSectionUpdateInputValidator : AbstractValidator<CourseSectionUpdateInput>
    {
        public CourseSectionUpdateInputValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Please enter the  Course title");
            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("Please put Description for this Course");
        }
    }
}