using System;
using DAL.Entities.Common;
using System.Collections.Generic;
namespace DAL.Entities.Categories
{
    public class Category:BaseEntity
    {
        public virtual Category BaseCategory { get; set; }
       
        public int? BaseCategoryID { get; set; }

        public string CategoryName { get; set; }
       
        public virtual ICollection<Category> subCategory { get; set; }


        public virtual ICollection<Courses.Course> Courses { get; set; }
    }
}