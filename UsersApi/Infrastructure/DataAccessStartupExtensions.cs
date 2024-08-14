using Microsoft.EntityFrameworkCore;
using UsersApi.Data.Contexts;

namespace UsersApi.Infrastructure
{
    public static class DataAccessStartupExtensions
    {
        public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.MigrationsAssembly("UsersApi.Data");
                    x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                })
                       .EnableSensitiveDataLogging();
            });
        }
    }
}
