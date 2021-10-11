using DAL.Entities.Common;
using DAL.Entities.Identity;
using DAL.Entities.Ratings.enums;

namespace DAL.Entities.Ratings
{
    public class Rating : BaseEntity
    {
        public RatingStar RatingStar { get; set; }
        ///  when course exist ..
    }
}