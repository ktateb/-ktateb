using DAL.Entities.Common;
using DAL.Entities.Students;

namespace DAL.Entities.CourseQuizes
{
    public class StudentAnswer : BaseEntity
    {
        public Student Student { get; set; }
        public int StudentId { get; set; }
        public SectionQuiz Quiz { get; set; }
        public int QuizId { get; set; }
        public QuizOptions Answer { get; set; }
        public int AnswerId { get; set; }
    }
}