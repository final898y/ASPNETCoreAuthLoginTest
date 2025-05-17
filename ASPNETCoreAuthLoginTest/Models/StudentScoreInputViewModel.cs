namespace ASPNETCoreAuthLoginTest.Models
{
    public class StudentScoreInputViewModel
    {
        public required int StudentID { get; set; }
        public required string Name { get; set; }
        public int? Score { get; set; } // 分數可為 null，表示尚未填寫
    }
    public class Course
    {
        public required int CourseID { get; set; }
        public required string CourseName { get; set; }
    }

    public class InputScoresViewModel
    {
        public List<Course> Courses { get; set; } = new();
    }

}
