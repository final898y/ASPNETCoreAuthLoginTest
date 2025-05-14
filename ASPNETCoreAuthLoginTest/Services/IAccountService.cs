using loginTest.Models;

namespace loginTest.Services
{
    public interface IAccountService
    {
        UserDto? ValidateUser(string username, string password);
        bool IsUserExists(string username);
        bool CreateUser(string username, string email, string password);
        UserDto GetUserByUsername(string username);
    }
}
