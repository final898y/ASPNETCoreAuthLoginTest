namespace loginTest.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; } // 實際應用中應存儲密碼哈希而不是明文密碼
        public required string Role { get; set; }
    }
}
