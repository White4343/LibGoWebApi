using Book.API.Data.Entities;
using Book.API.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Books> Books { get; set; }
        public DbSet<Comments> Comments { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BooksEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CommentsEntityConfiguration());
        }
    }
}