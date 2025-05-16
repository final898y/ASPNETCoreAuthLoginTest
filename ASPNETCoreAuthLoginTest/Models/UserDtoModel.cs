namespace  ASPNETCoreAuthLoginTest.Models
{
    public class UserLoginDto
    {
        public required string AccountName { get; set; }
        public required string PasswordHash { get; set; } 
        public required string Role { get; set; }
        public required string UserName { get; set; }

    }
    public enum UserRole
    {
        Teacher,
        Student
    }


}
