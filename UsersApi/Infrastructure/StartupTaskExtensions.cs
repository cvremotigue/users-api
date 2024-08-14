using Microsoft.EntityFrameworkCore;
using UsersApi.Data.Contexts;

namespace UsersApi.Infrastructure
{
    public static class StartupTaskExtensions
    {
        public static void ExecuteStartupTasks(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<UserDbContext>();
                if (!context.Database.IsSqlServer())
                {
                    return;
                }

                SetupDatabase(context);
            }
        }

        public static void SetupDatabase(UserDbContext context)
        {
            context.SkipInitialMigrationIfDatabaseAlreadyExists();
            context.Database.Migrate();
        }
    }
}
