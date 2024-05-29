using Book.API.Data;
using Book.API.Data.Entities;
using Book.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Repositories
{
    public class BookGenresRepository : IBookGenresRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookGenresRepository> _logger;

        public BookGenresRepository(AppDbContext context, ILogger<BookGenresRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<BookGenres> CreateBookGenreAsync(BookGenres bookGenre)
        {
            try
            {
                var result = await _context.BookGenres.AddAsync(bookGenre);

                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<BookGenres> GetBookGenreByIdAsync(int id)
        {
            try
            {
                var result = await _context.BookGenres.FindAsync(id);

                if (result == null)
                    throw new NotFoundException($"Book genre with id {id} not found");

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BookGenres>> GetBookGenresAsync()
        {
            try
            {
                var result = await _context.BookGenres.ToListAsync();

                if (result == null || result.Count == 0)
                {
                    throw new NotFoundException("No book genres found");
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BookGenres>> GetBookGenresByBookIdAsync(int bookId)
        {
            try
            {
                var result = await _context.BookGenres.Where(bg => bg.BookId == bookId).ToListAsync();

                if (result == null || result.Count == 0)
                {
                    throw new NotFoundException($"No book genres found for book with id {bookId}");
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BookGenres>> GetBookGenresByGenreIdAsync(int genreId)
        {
            try
            {
                var result = await _context.BookGenres.Where(bg => bg.GenreId == genreId).ToListAsync();

                if (result == null || result.Count == 0)
                {
                    throw new NotFoundException($"No book genres found for genre with id {genreId}");
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<BookGenres> UpdateBookGenreAsync(BookGenres bookGenre)
        {
            try
            {
                await GetBookGenreByIdAsync(bookGenre.Id);

                var result = _context.BookGenres.Update(bookGenre);

                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteBookGenreAsync(int id)
        {
            try
            {
                var bookGenre = await GetBookGenreByIdAsync(id);

                _context.BookGenres.Remove(bookGenre);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
