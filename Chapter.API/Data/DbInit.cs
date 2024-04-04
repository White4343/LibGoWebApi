using Chapter.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chapter.API.Data
{
    public class DbInit
    {
        private static IEnumerable<Chapters> GetPreconfiguredChapters()
        {
            return new List<Chapters>
            {
                new Chapters { Title = "Chapter 1", Content = "Content 1", IsFree = true, CreatedAt = DateTime.UtcNow, UpdatedAt = null, BookId = 1 },
                new Chapters { Title = "Chapter 2", Content = "Content 2", IsFree = true, CreatedAt = DateTime.UtcNow, UpdatedAt = null, BookId = 1 },
                new Chapters { Title = "Chapter 3", Content = "Content 3", IsFree = true, CreatedAt = DateTime.UtcNow, UpdatedAt = null, BookId = 1 },
                new Chapters { Title = "Chapter 1", Content = "Content 1", IsFree = true, CreatedAt = DateTime.UtcNow, UpdatedAt = null, BookId = 2 },
                new Chapters { Title = "Chapter 2", Content = "Content 2", IsFree = true, CreatedAt = DateTime.UtcNow, UpdatedAt = null, BookId = 2 },
                new Chapters { Title = "Chapter 3", Content = "Content 3", IsFree = true, CreatedAt = DateTime.UtcNow, UpdatedAt = null, BookId = 2 }
            };
        }

        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.Chapters.AnyAsync())
            {
                await context.Chapters.AddRangeAsync(GetPreconfiguredChapters());
            }

            await context.SaveChangesAsync();
        }
    }
}