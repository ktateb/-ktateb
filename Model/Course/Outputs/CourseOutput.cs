namespace Model.Course.Outputs
{
    public class CourseOutput
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string Description { get; set; }
        public string LearnListDescription { get; set; }
        public string ThisCourseFor { get; set; }
        public string PreRequerment { get; set; }
        public int CategoryId { get; set; }
        public int CategoryName{ get; set; }
        public int TeacherUserName { get; set; }
        public int TeacherDisplayrName { get; set; }
    }
}