using Admin.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.API.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<BoughtBooks> BoughtBooks { get; set; }
        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<UserSubscriptions> UserSubscriptions { get; set; }
    }
}
