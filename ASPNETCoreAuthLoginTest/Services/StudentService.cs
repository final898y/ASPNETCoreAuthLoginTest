using ASPNETCoreAuthLoginTest.Repositorys;
using ASPNETCoreAuthLoginTest.Models;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using ASPNETCoreAuthLoginTest.Repos;

namespace  ASPNETCoreAuthLoginTest.Services
{
    public class StudentService : IUserService<StudentProfileViewModel>
    {
        private readonly IUserRepos<StudentProfileViewModel> _studentRepos;

        // 使用建構式注入
        public StudentService(IUserRepos<StudentProfileViewModel> studentRepos)
        {
            _studentRepos = studentRepos;
        }

        public StudentProfileViewModel? GetUserProfile(string accountName)
        {
            var queryResult = _studentRepos.GetUserProfile(accountName);
            if (queryResult != null)
            {
                return queryResult;
            }

            return null;
        }


      
    
    }
}
