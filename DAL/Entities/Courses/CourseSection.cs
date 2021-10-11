using System.Collections.Generic;
using System; 
using DAL.Entities.Common;
namespace DAL.Entities.Courses
{
    public class CourseSection:BaseEntity
    {
        public String SectionTitle { get; set; }
        
        public String ShortDescription { get; set; } 

        public virtual Course Course { get; set; }
        
        public int CourseId { get;set; }

        public virtual ICollection<CourseQuizes.SectionQuiz>   Quizzes { get; set; }
        
        public virtual ICollection<CourseVedio> Vedios{ get; set; }
    }
}