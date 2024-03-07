using Book.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Data
{
    public class DbInit
    {
        private static IEnumerable<Books> GetPreconfiguredBooks()
        {
            return new List<Books>
            {
                new Books { Name = "The Hitchhiker's Guide to the Galaxy", Description = "The Hitchhiker's Guide to the Galaxy is a comedy science fiction series created by Douglas Adams.", Price = 19.99m, PublishDate = DateTime.UtcNow, UserId = 1, CoAuthorIds = null },
                new Books { Name = "The Restaurant at the End of the Universe", Description = "The Restaurant at the End of the Universe is the second book in the Hitchhiker's Guide to the Galaxy series by Douglas Adams.", Price = 15.99m, PublishDate = DateTime.UtcNow, UserId = 1, CoAuthorIds = null },
                new Books { Name = "Life, the Universe and Everything", Description = "Life, the Universe and Everything is the third book in the five-volume Hitchhiker's Guide to the Galaxy science fiction series by British writer Douglas Adams.", Price = 12.99m, PublishDate = DateTime.UtcNow, UserId = 1, CoAuthorIds = null },
                new Books { Name = "So Long, and Thanks for All the Fish", Description = "So Long, and Thanks for All the Fish is the fourth book of the Hitchhiker's Guide to the Galaxy trilogy written by Douglas Adams.", Price = 10.99m, PublishDate = DateTime.UtcNow, UserId = 1, CoAuthorIds = null },
                new Books { Name = "Mostly Harmless", Description = "Mostly Harmless is a novel by Douglas Adams and the fifth book in the Hitchhiker's Guide to the Galaxy series.", Price = 8.99m, PublishDate = DateTime.UtcNow, UserId = 1, CoAuthorIds = null }
            };
        }

        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.Books.AnyAsync())
            {
                await context.Books.AddRangeAsync(GetPreconfiguredBooks());
                await context.SaveChangesAsync();
            }
        }
    }
}