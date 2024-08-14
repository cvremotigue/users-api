using Microsoft.EntityFrameworkCore;
using UsersApi.Core.Entities;

namespace UsersApi.Data.Contexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<RunningActivities> RunningActivities { get; set; }
    }
}
