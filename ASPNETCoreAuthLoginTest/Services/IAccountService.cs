using  ASPNETCoreAuthLoginTest.Models;

namespace  ASPNETCoreAuthLoginTest.Services
{
    public interface IAccountService
    {
        UserLoginDto? ValidateUser(string accountName, string password,UserRole role);
        bool IsUserExists(string accountName);
        bool CreateUser(string accountName, string email, string password);
    }
}
