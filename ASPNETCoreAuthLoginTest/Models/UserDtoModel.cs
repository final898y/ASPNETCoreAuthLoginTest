namespace  ASPNETCoreAuthLoginTest.Models
{
    public class UserDto
    {
        public string? userID { get; set; }
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
