using Model.Helper;

namespace Model.Course.Inputs
{
    public class CourseOrderParam:Paging
    {
        public int OrderType { get; set; }
    }
}