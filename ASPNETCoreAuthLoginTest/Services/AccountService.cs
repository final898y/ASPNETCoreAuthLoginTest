using ASPNETCoreAuthLoginTest.Repositorys;
using  ASPNETCoreAuthLoginTest.Models;
using System.Reflection.PortableExecutable;
using System.Security.Claims;

namespace  ASPNETCoreAuthLoginTest.Services
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepos _AccountRepos;

        // 使用建構式注入
        public AccountService(AccountRepos accountRepos)
        {
            _AccountRepos = accountRepos;
        }

        public UserLoginDto? ValidateUser(string accountName, string password,UserRole role)
        {
            var queryResult = _AccountRepos.GetUserDto(accountName, role);

            // 檢查使用者是否存在，以及密碼是否驗證成功
            if (queryResult != null && BCrypt.Net.BCrypt.Verify(password, queryResult.PasswordHash))
            {
                return queryResult;
            }

            return null;
        }


        public bool IsUserExists(string accountName)
        {
            return true;
        }

        public bool CreateUser(string accountName, string email, string password)
        {
            return true;
        }
    
    }
}
