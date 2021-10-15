using System;
using DAL.Entities.Common;
using System.Collections.Generic;
using DAL.Entities.Courses;

namespace DAL.Entities.Categories
{
    public class Category : BaseEntity
    {
        public virtual Category BaseCategory { get; set; }
        public int? BaseCategoryID { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<Category> SubCategory { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}