using ASPNETCoreAuthLoginTest.Repositorys;
using ASPNETCoreAuthLoginTest.Models;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using ASPNETCoreAuthLoginTest.Repos;

namespace  ASPNETCoreAuthLoginTest.Services
{
    public class TeacherService : IUserService<TeacherProfileViewModel>
    {
        private readonly IUserRepos<TeacherProfileViewModel> _teachertRepos;

        // 使用建構式注入
        public TeacherService(IUserRepos<TeacherProfileViewModel> teachertRepos)
        {
            _teachertRepos = teachertRepos;
        }

        public TeacherProfileViewModel? GetUserProfile(string accountName)
        {
            var queryResult = _teachertRepos.GetUserProfile(accountName);
            if (queryResult != null)
            {
                return queryResult;
            }

            return null;
        }


      
    
    }
}
