namespace ASPNETCoreAuthLoginTest.Models
{
    public class UserProfileViewModel
    {
        public required string UserName { get; set; }
        public required string Role { get; set; }
        public required string AccountName { get; set; }

    }
    public class TeacherProfileViewModel: UserProfileViewModel
    {
        public required string Subject { get; set; }
        public required string LeadClass { get; set; }
    }
    public class StudentProfileViewModel : UserProfileViewModel
    {
        public required string Grade { get; set; }
        public required string Class { get; set; }
    }
}
