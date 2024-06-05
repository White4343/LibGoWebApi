using Admin.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.API.Data
{
    public class ChapterDbContext : DbContext
    {
        public ChapterDbContext(DbContextOptions<ChapterDbContext> options) : base(options)
        {
        }

        public DbSet<Chapters> Chapters { get; set; }
    }
}
