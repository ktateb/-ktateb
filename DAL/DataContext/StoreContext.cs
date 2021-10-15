using DAL.Entities.Categories;
using DAL.Entities.Comments;
using DAL.Entities.Countries;
using DAL.Entities.CourseQuizes;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using DAL.Entities.Messages;
using DAL.Entities.Ratings;
using DAL.Entities.Reports;
using DAL.Entities.StudentCourses;
using DAL.Entities.StudentFavoriteCourses;
using DAL.Entities.Teachers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataContext
{
    public class StoreContext : IdentityDbContext<User>
    {
        public StoreContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<QuizOptions> QuizOptions { get; set; }
        public DbSet<SectionQuiz> SectionQuizzes { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseSection> CourseSections { get; set; }
        public DbSet<CourseVedio> CourseVedios { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ReportComment> ReportComments { get; set; }
        public DbSet<ReportCourse> ReportCourses { get; set; }
        public DbSet<ReportMessage> ReportMessages { get; set; }
        public DbSet<ReportSubComment> ReportSubComments { get; set; }
        public DbSet<ReportUser> ReportUsers { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<StudentFavoriteCourse> StudentFavoriteCourses { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(u => u.UserRecivers)
                .HasForeignKey(fk => fk.SenderId);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Reciver)
                .WithMany(u => u.UserSenders)
                .HasForeignKey(fk => fk.ReciverId);

            modelBuilder.Entity<ReportUser>()
                .HasOne(u => u.UserSendReport)
                .WithMany(u => u.UsersReciveReportFromUser)
                .HasForeignKey(fk => fk.UserSendReportId);

            modelBuilder.Entity<ReportUser>()
                .HasOne(u => u.UserReciveReport)
                .WithMany(u => u.UsersSendReportToUser)
                .HasForeignKey(fk => fk.UserReciveReportId);

            modelBuilder.Entity<User>()
                .Property(x => x.FirstName).IsUnicode();
            modelBuilder.Entity<User>()
                .Property(x => x.LastName).IsUnicode();
            modelBuilder.Entity<Teacher>()
            .Property(x => x.AboutMe).IsUnicode();
            base.OnModelCreating(modelBuilder);
        }
    }
}
