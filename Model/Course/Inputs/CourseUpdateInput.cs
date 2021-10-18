using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Model.Course.Inputs
{
    public class CourseUpdateInput
    {
        public int Id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string Description { get; set; }
        public string LearnListDescription { get; set; }
        public string ThisCourseFor { get; set; }
        public string PreRequerment { get; set; } 
        public int CategoryId { get; set; }
    }
    public class CourseUpdateInputValidator : AbstractValidator<CourseUpdateInput>
    {
        public CourseUpdateInputValidator()
        {
            RuleFor(x => x.title)
                .NotEmpty().WithMessage("Please enter the  Course title");
            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("Please put Description for this Course");
        }
    }
}