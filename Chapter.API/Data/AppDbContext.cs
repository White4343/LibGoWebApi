using Chapter.API.Data.Entities;
using Chapter.API.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Chapter.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Chapters> Chapters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChaptersEntityConfiguration());
        }
    }
}