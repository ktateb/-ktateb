namespace Model.Course.Outputs
{
    public class CourseOutput
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearnListDescription { get; set; }
        public bool IsOwned { get; set; }=false;
        public string ThisCourseFor { get; set; }
        public string PreRequerment { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string TeacherUserName { get; set; }
        public string TeacherDisplayrName { get; set; }
    }
}