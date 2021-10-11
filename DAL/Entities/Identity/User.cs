using DAL.Entities.Comments;
using DAL.Entities.Messages;
using DAL.Entities.Ratings;
using DAL.Entities.Reports;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
        public string PictureUrl { get; set; }
        public DateTime StartRegisterDate { get; set; }
        public virtual ICollection<Message> UserSenders { get; set; }
        public virtual ICollection<Message> UserRecivers { get; set; }
        public virtual ICollection<ReportUser> UserSendReportToUsers { get; set; }
        public virtual ICollection<ReportUser> UserReciveReportFromUsers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<SubComment> SubComments { get; set; }

        [NotMapped]
        public string FullName { get => FirstName + " " + LastName; }
    }
}