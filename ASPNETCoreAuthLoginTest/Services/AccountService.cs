using ASPNETCoreAuthLoginTest.Repositorys;
using loginTest.Models;
using System.Reflection.PortableExecutable;

namespace loginTest.Services
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepos _AccountRepos;

        // 使用建構式注入
        public AccountService(AccountRepos accountRepos)
        {
            _AccountRepos = accountRepos;
        }

        private List<UserDto> _users = new List<UserDto>
        {
            //password=123456
            new UserDto { Username = "tt",PasswordHash = "$2a$11$bX3apy3i3kcxnC4eAtgC2.jGDj/C6VcYr6T33Fv1xihL.T6yhaRNm", Role = "teacher" },
            //password=test1234
            new UserDto { Username = "ss",PasswordHash = "$2a$11$bX3apy3i3kcxnC4eAtgC2.jGDj/C6VcYr6T33Fv1xihL.T6yhaRNm", Role = "student" }

        };

        public UserDto? ValidateUser(string username, string password)
        {
            var queryResult = _AccountRepos.GetTeacherUserDto(username);

            // 檢查使用者是否存在，以及密碼是否驗證成功
            if (queryResult != null && BCrypt.Net.BCrypt.Verify(password, queryResult.PasswordHash))
            {
                return queryResult;
            }

            return null;
        }


        public bool IsUserExists(string username)
        {
            return true;
        }

        public bool CreateUser(string username, string email, string password)
        {
            return true;
        }

        public UserDto GetUserByUsername(string username)
        {
            return new UserDto
            {
                Username = "",
                PasswordHash = "",
                Role = "",
            };
        }
    }
}
