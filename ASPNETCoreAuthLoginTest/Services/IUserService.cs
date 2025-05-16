using  ASPNETCoreAuthLoginTest.Models;

namespace  ASPNETCoreAuthLoginTest.Services
{
    public interface IUserService<TProfile> where TProfile : UserProfileViewModel
    {
        TProfile? GetUserProfile(string accountName);
    }
}
