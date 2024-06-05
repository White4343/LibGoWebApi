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
                new Books { Name = "The Hitchhiker's Guide to the Galaxy", 
                    Description = "The Hitchhiker's Guide to the Galaxy is a comedy science fiction series created by Douglas Adams.", 
                    Price = 190, PhotoUrl = $"https://picsum.photos/id/1/500", IsVisible = true, IsAvailableToBuy = true, PublishDate = DateTime.UtcNow, UserId = 1, CoAuthorIds = null },
                new Books { Name = "The Restaurant at the End of the Universe", 
                    Description = "The Restaurant at the End of the Universe is the second book in the Hitchhiker's Guide to the Galaxy series by Douglas Adams.", 
                    Price = 150, PhotoUrl = $"https://picsum.photos/id/2/500", IsVisible = true, IsAvailableToBuy = false, PublishDate = DateTime.UtcNow, UserId = 1, CoAuthorIds = null },
                new Books { Name = "Life, the Universe and Everything", 
                    Description = "Life, the Universe and Everything is the third book in the five-volume Hitchhiker's Guide to the Galaxy science fiction series by British writer Douglas Adams.", 
                    Price = 120, PhotoUrl = $"https://picsum.photos/id/3/500", IsVisible = true, IsAvailableToBuy = true, PublishDate = DateTime.UtcNow, UserId = 1, CoAuthorIds = null },
                new Books { Name = "So Long, and Thanks for All the Fish", 
                    Description = "So Long, and Thanks for All the Fish is the fourth book of the Hitchhiker's Guide to the Galaxy trilogy written by Douglas Adams.", 
                    Price = 100, PhotoUrl = $"https://picsum.photos/id/4/500", IsVisible = true, IsAvailableToBuy = true, PublishDate = DateTime.UtcNow, UserId = 1, CoAuthorIds = null },
                new Books { Name = "Mostly Harmless", 
                    Description = "Mostly Harmless is a novel by Douglas Adams and the fifth book in the Hitchhiker's Guide to the Galaxy series.", 
                    Price = 80, PhotoUrl = $"https://picsum.photos/id/5/500", IsVisible = false, IsAvailableToBuy = true, PublishDate = DateTime.UtcNow, UserId = 1, CoAuthorIds = null }
            };
        }

        private static IEnumerable<Comments> GetPreconfiguredComments()
        {
            return new List<Comments>
            {
                new Comments { Content = "This is a great book!",
                    UserNickname = "John Doe", UserPhotoUrl = null,
                    CreateDate =  DateTime.UtcNow, UpdateDate = null, UserId = 1, BookId = 1 },
                new Comments { Content = "I love this book!",
                    UserNickname = "John Dust", UserPhotoUrl = null,
                    CreateDate =  DateTime.UtcNow, UpdateDate = null, UserId = 1, BookId = 1 },
                new Comments { Content = "This is a great book!",
                    UserNickname = "John Winter", UserPhotoUrl = null,
                    CreateDate =  DateTime.UtcNow, UpdateDate = null, UserId = 1, BookId = 1 },
                new Comments { Content = "I love this book!",
                    UserNickname = "John Lover", UserPhotoUrl = null,
                    CreateDate =  DateTime.UtcNow, UpdateDate = null, UserId = 1, BookId = 1 },
                new Comments { Content = "This is a great book!",
                    UserNickname = "John Spring", UserPhotoUrl = null,
                    CreateDate =  DateTime.UtcNow, UpdateDate = null, UserId = 1, BookId = 1 },
                new Comments { Content = "I love this book!",
                    UserNickname = "John Summer", UserPhotoUrl = null,
                    CreateDate =  DateTime.UtcNow, UpdateDate = null, UserId = 1, BookId = 1 },
                new Comments { Content = "This is a great book!",
                    UserNickname = "John von Buren", UserPhotoUrl = null,
                    CreateDate =  DateTime.UtcNow, UpdateDate = null, UserId = 1, BookId = 1 },
                new Comments { Content = "I love this book!",
                    UserNickname = "John Astrea", UserPhotoUrl = null,
                    CreateDate =  DateTime.UtcNow, UpdateDate = null, UserId = 1, BookId = 1 },
                new Comments { Content = "This is a great book!",
                    UserNickname = "John Subaru", UserPhotoUrl = null,
                    CreateDate =  DateTime.UtcNow, UpdateDate = null, UserId = 1, BookId = 1 },
                new Comments { Content = "I love this book!",
                    UserNickname = "John Love", UserPhotoUrl = null,
                    CreateDate =  DateTime.UtcNow, UpdateDate = null, UserId = 1, BookId = 1 }
            };
        }

        private static IEnumerable<Readers> GetPreconfiguredReaders()
        {
            return new List<Readers>
            {
                new Readers { Status = "Reading", IsVisible = true, NotifyEnabled = true, Rating = 5, UserId = 3, BookId = 1, ChapterId = 1 },
                new Readers { Status = "Abandoned", IsVisible = false, NotifyEnabled = true, Rating = 1, UserId = 3, BookId = 2, ChapterId = 2 }
            };
        }

        private static IEnumerable<Genres> GetPreconfiguredGenres()
        {
            return new List<Genres>
            {
                new Genres { Name = "Science Fiction" },
                new Genres { Name = "Fantasy" },
                new Genres { Name = "Mystery" },
                new Genres { Name = "Thriller" },
                new Genres { Name = "Romance" },
                new Genres { Name = "Western" },
                new Genres { Name = "Dystopian" },
                new Genres { Name = "Contemporary" },
                new Genres { Name = "Horror" },
                new Genres { Name = "Literary Fiction" }
            };
        }

        private static IEnumerable<BookGenres> GetPreconfiguredBookGenres()
        {
            return new List<BookGenres>
            {
                new BookGenres { BookId = 1, GenreId = 1 },
                new BookGenres { BookId = 1, GenreId = 2 },
                new BookGenres { BookId = 2, GenreId = 3 },
                new BookGenres { BookId = 2, GenreId = 4 },
                new BookGenres { BookId = 3, GenreId = 5 },
                new BookGenres { BookId = 3, GenreId = 6 },
                new BookGenres { BookId = 4, GenreId = 7 },
                new BookGenres { BookId = 4, GenreId = 8 },
                new BookGenres { BookId = 5, GenreId = 9 },
                new BookGenres { BookId = 5, GenreId = 10 }
            };
        }

        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.Books.AnyAsync())
            {
                await context.Books.AddRangeAsync(GetPreconfiguredBooks());
            }

            if (!await context.Comments.AnyAsync())
            {
                await context.Comments.AddRangeAsync(GetPreconfiguredComments());
            }

            if (!await context.Readers.AnyAsync())
            {
                await context.Readers.AddRangeAsync(GetPreconfiguredReaders());
            }

            if (!await context.Genres.AnyAsync())
            {
                await context.Genres.AddRangeAsync(GetPreconfiguredGenres());
            }

            if (!await context.BookGenres.AnyAsync())
            {
                await context.BookGenres.AddRangeAsync(GetPreconfiguredBookGenres());
            }

            await context.SaveChangesAsync();
        }
    }
}