using DAL.Entities.Common;
namespace DAL.Entities.CourseQuizes
{
    public class QuizOptions : BaseEntity
    {
        public virtual SectionQuiz Quiz { get; set; }
        public int QuizId { get; set; }
        public string OptionText { get; set; }
        public bool istrue { get; set; }
    }
}