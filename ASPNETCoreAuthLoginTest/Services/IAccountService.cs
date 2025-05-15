using  ASPNETCoreAuthLoginTest.Models;

namespace  ASPNETCoreAuthLoginTest.Services
{
    public interface IAccountService
    {
        UserDto? ValidateUser(string username, string password,UserRole role);
        bool IsUserExists(string username);
        bool CreateUser(string username, string email, string password);
        UserDto? GetUserByUsername(string username, UserRole role);
    }
}
