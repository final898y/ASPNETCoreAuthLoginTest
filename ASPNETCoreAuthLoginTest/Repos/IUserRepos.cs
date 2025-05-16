using ASPNETCoreAuthLoginTest.Models;

namespace ASPNETCoreAuthLoginTest.Repos
{
    public interface IUserRepos<TProfile> where TProfile : UserProfileViewModel
    {
        TProfile? GetUserProfile(string accountName);
    }
}
