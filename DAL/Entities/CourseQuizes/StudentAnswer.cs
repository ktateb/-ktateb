using DAL.Entities.Common;
using DAL.Entities.Identity;

namespace DAL.Entities.CourseQuizes
{
    public class StudentAnswer : BaseEntity
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public SectionQuiz Quiz { get; set; }
        public int QuizId { get; set; }
        public QuizOptions Answer { get; set; }
        public int AnswerId { get; set; }
    }
}