using Admin.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.API.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Books> Books { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Readers> Readers { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<BookGenres> BookGenres { get; set; }
    }
}
