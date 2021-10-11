using DAL.Entities.Common;
namespace DAL.Entities.CourseQuizes
{
    public class StudentAnswer:BaseEntity
    {
        public Students.Student Student { get; set; }
        public int StudentId { get; set; }

        public CourseQuizes.SectionQuiz Quiz  { get; set; }

        public int QuizId { get; set; }

        public QuizOptions Answer { get; set; }
        
        public int AnswerId { get; set; }
    }
}