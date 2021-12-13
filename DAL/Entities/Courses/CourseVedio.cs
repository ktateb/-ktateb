using System;
using System.Collections.Generic;
using DAL.Entities.Common;
using DAL.Entities.StudentWatches;

namespace DAL.Entities.Courses
{
    public class CourseVedio : BaseEntity
    {
        public virtual CourseSection Section { get; set; }
        public int SectionId { get; set; }
        public String VedioTitle { get; set; }
        public String ShortDescription { get; set; }
        public int TimeInSeconds { get; set; }
        public String URL { get; set; }
        public String ImgeURL { get; set; }
        public DateTime AddedDate { get; set; }
        public virtual ICollection<StudentWatchedVedio> WatchedByList { get; set; }
    }
}