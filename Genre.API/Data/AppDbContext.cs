using Genre.API.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Genre.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.BookGenres> BookGenres { get; set; }
        public DbSet<Entities.Genres> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GenresEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BookGenresEntityConfiguration());
        }
    }
}