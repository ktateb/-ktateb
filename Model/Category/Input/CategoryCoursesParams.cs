using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Model.Helper;

namespace Model.Category.Input
{

    public class CategoryCoursesParams : Paging
    {
        /// <summary>
        /// popular = 1, Rating= 2, Price= 3,Student= 4
        /// </summary> 

        public enum Ordaring { popular = 1, Rating, Price, Student }
        /// <summary>
        /// popular = 1, Rating= 2, Price= 3,Student= 4
        /// </summary> 
        public Ordaring Orderby { get; set; }
        public int LowerPrice { get; set; }
        public int HigherPrice { get; set; }
    }
    public class CategoryCoursesParamsValidator : AbstractValidator<CategoryCoursesParams>
    {
        public CategoryCoursesParamsValidator()
        {
            RuleFor(x => x.LowerPrice).GreaterThanOrEqualTo(0).WithMessage("Lower Price must be greater than or equal to zero");
            RuleFor(x => x.HigherPrice).GreaterThanOrEqualTo(x => x.LowerPrice).WithMessage("Higher Price  must be greater than or equal to LowPrice");

        }
    }
}