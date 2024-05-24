using Microsoft.EntityFrameworkCore;
using User.API.Data.Entities;
using User.API.Data.EntityConfigurations;

namespace User.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<BoughtBooks> BoughtBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BoughtBooksEntityConfiguration());
        }
    }
}
