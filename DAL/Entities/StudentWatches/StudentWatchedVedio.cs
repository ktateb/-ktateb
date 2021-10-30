using System;
using DAL.Entities.Common;
using DAL.Entities.Courses;
using DAL.Entities.Identity;

namespace DAL.Entities.StudentWatches
{
    public class StudentWatchedVedio : BaseEntity
    {
        public DateTime WatchedDate { get; set; }
        public virtual User User { get; set; }
        public string UsertId { get; set; }
        public virtual CourseVedio Vedio { get; set; }
        public int VedioId { get; set; }
    }
}