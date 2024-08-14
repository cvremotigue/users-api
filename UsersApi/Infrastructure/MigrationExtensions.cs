using Dapper;
using Microsoft.EntityFrameworkCore;
using UsersApi.Data.Contexts;
using MicrosoftSql = Microsoft.Data.SqlClient;
using SystemSql = System.Data.SqlClient;

namespace UsersApi.Infrastructure
{
    public static class MigrationExtensions
    {
        public static void SkipInitialMigrationIfDatabaseAlreadyExists(this UserDbContext context)
        {
            try
            {
                var connection = context.Database.GetDbConnection();

                var initialMigrationAlreadyApplied = connection.QuerySingle<int?>("SELECT OBJECT_ID('Users')").HasValue;

                if (initialMigrationAlreadyApplied)
                {
                    var parameters = new
                    {
                        MigrationId = "20240814145219_InitialMigration",
                        ProductVersion = "1.0.0-user-api"
                    };

                    if (!connection.QuerySingleOrDefault<int?>(
                                       $"SELECT 1 FROM dbo.__EFMigrationsHistory WHERE MigrationId = '{parameters.MigrationId}'")
                                   .HasValue)
                    {
                        connection.Execute(
                            "INSERT INTO dbo.__EFMigrationsHistory(MigrationId, ProductVersion) VALUES(@MigrationId, @ProductVersion)",
                            parameters);
                    }
                }
            }
            catch (Exception ex) when (ex is SystemSql.SqlException or MicrosoftSql.SqlException && ex.Message.Contains("Cannot open database"))
            {
                // Database doesn't exist and needs to be migrated. 
                return;
            }
        }
    }
}
