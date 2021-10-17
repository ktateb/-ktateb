using FluentValidation;

namespace Model.Report.Course.Inputs
{
    public class ReportCourseInput
    {
        public string Text { get; set; }
        public int CourseId { get; set; }
    }
    public class ReportCourseInputValidator : AbstractValidator<ReportCourseInput>
    {
        public ReportCourseInputValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Please enter why you report this course");
            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("Please enter the course you will report it");
        }
    }
}