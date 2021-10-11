using System;
using DAL.Entities.Common;

namespace DAL.Entities.StudentWatches
{
    public class StudentWatchedVedio:BaseEntity
    {
        public DateTime WatchedDate { get; set; }

        public virtual Students.Student Student { get; set; }

        public int StudentId { get; set; }

        public virtual Courses.CourseVedio Vedio { get; set; }

        public int VedioId { get; set; }
    }
}