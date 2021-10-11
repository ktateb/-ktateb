using DAL.Entities.Common;
using DAL.Entities.Courses;
using System.Collections.Generic;
namespace DAL.Entities.CourseQuizes
{
    public class SectionQuiz : BaseEntity
    {
        public virtual CourseSection Section { get; set; }
        public int SectionId { get; set; }
        public string QuizText { get; set; }
        public virtual ICollection<QuizOptions> Options { get; set; }
    }
}