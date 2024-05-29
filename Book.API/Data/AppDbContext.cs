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
        public DbSet<Readers> Readers { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<BookGenres> BookGenres { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BooksEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CommentsEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ReadersEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GenresEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BooksGenresEntityConfiguration());
        }
    }
}