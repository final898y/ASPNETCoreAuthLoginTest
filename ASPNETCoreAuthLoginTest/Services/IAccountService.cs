using loginTest.Models;

namespace loginTest.Services
{
    public interface IAccountService
    {
        User? ValidateUser(string username, string password);
        bool IsUserExists(string username);
        bool CreateUser(string username, string email, string password);
        User GetUserByUsername(string username);
    }
}
