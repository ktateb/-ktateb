namespace Model.Helper
{
    public class Paging
    {
        private const int maxPageSize = 10;
        private int pageSize = 6;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            set { pageSize = (value > maxPageSize) ? 10 : value; }
            get => pageSize;
        }
    }
}