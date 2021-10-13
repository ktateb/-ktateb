using DAL.Entities.Identity;
using DAL.Entities.Messages;
using DAL.Entities.Reports;
using DAL.Entities.Teachers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataContext
{
    public class StoreContext : IdentityDbContext<User>
    {
        public StoreContext(DbContextOptions options) : base(options)
        { }

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
