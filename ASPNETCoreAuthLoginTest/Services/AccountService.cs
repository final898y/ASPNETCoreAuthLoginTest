using loginTest.Models;

namespace loginTest.Services
{
    public class AccountService : IAccountService
    {
        private List<User> _users = new List<User>
        {
            //password=123456
            new User { Id = 1, Username = "tt", Email = "tt@gmail.com", PasswordHash = "$2a$11$bX3apy3i3kcxnC4eAtgC2.jGDj/C6VcYr6T33Fv1xihL.T6yhaRNm", Role = "teacher" },
            //password=test1234
            new User { Id = 2, Username = "ss", Email = "ss@gmail.com", PasswordHash = "$2a$11$bX3apy3i3kcxnC4eAtgC2.jGDj/C6VcYr6T33Fv1xihL.T6yhaRNm", Role = "student" }

        };

        public User? ValidateUser(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);

            // 檢查使用者是否存在，以及密碼是否驗證成功
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }


        public bool IsUserExists(string username)
        {
            return _users.Any(u => u.Username == username);
        }

        public bool CreateUser(string username, string email, string password)
        {
            var id = _users.Max(u => u.Id) + 1;
            _users.Add(new User
            {
                Id = id,
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password), // 實際應用中應使用密碼哈希
                Role = "User"
            });
            return true;
        }

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }
    }
}
