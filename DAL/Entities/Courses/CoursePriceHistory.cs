using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities.Common;

namespace DAL.Entities.Courses
{
    public class CoursePriceHistory:BaseEntity
    {
        public double Price { get; set; }
        public virtual Course Course { get; set; }
        public int CourseId {get;set;}
        public DateTime StartedApplyDate { get; set; }
    }
}