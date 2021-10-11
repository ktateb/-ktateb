using System;
using DAL.Entities.Common;
using DAL.Entities.Courses;
using DAL.Entities.Students;

namespace DAL.Entities.StudentWatches
{
    public class StudentWatchedVedio : BaseEntity
    {
        public DateTime WatchedDate { get; set; }
        public virtual Student Student { get; set; }
        public int StudentId { get; set; }
        public virtual CourseVedio Vedio { get; set; }
        public int VedioId { get; set; }
    }
}