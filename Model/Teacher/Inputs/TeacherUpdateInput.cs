using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Model.Teacher.Inputs
{
    public class TeacherUpdateInput
    {
        [Required]
        public string AboutMe { get; set; }
        public string LinkedInUrl { get; set; }
        public string WhatsappUrl { get; set; }
        public string FaceBookUrl { get; set; }
        public string TelegramUrl { get; set; }
        [Required]
        public string Specialist { get; set; }
    }
    public class TeacherUpdateInputValidator : AbstractValidator<TeacherUpdateInput>
    {
        public TeacherUpdateInputValidator()
        {
            RuleFor(x => x.AboutMe)
                .NotEmpty().WithMessage("Please enter info About you");
            RuleFor(x => x.Specialist)
               .NotEmpty().WithMessage("Please enter Specialist");
        }
    }
}