namespace Model.Rating.Outputs
{
    public class RatingOutput
    {
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public long NumberOneStars { get; set; }
        public long NumberTwoStars { get; set; }
        public long NumberThreeStars { get; set; }
        public long NumberFourStars { get; set; }
        public long NumberFiveStars { get; set; }
        public double Rate { get; set; }
    }
}