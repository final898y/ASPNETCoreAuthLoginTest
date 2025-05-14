using ASPNETCoreAuthLoginTest.Repositorys;
using loginTest.Services;
using Microsoft.Data.Sqlite;

namespace ASPNETCoreAuthLoginTest.Utils
{
    public static class ServiceDI
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("Default");
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = connectionString,
                ForeignKeys = true,
                DefaultTimeout = 30
            };

            if (connectionString != null)
            {
                services.AddScoped<AccountRepos>(provider => new AccountRepos(connectionString));
            }
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 註冊應用邏輯層（Service）
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
