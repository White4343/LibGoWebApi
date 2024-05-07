using AutoMapper;
using Book.API.Data;
using Book.API.Data.Entities;
using Book.API.Models.Requests;
using Book.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BooksRepository> _logger;
        private readonly IMapper _mapper;

        public BooksRepository(AppDbContext context, ILogger<BooksRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<Books> CreateBookAsync(Books book)
        {
            try
            {
                var booksToCreate = await _context.Books.AddAsync(book);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Book {book.Name} with id {book.Id} is created");

                return booksToCreate.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Books> GetBookByIdAsync(int id)
        {
            try
            {
                var book = await BookExists(id);

                return book;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Books>> GetBooksAsync()
        {
            var books = await _context.Books.ToListAsync();

            if (books == null || books.Count == 0)
            {
                throw new NotFoundException("No books found.");
            }

            var resultBooks = books.Where(b => b.IsVisible).ToList();

            return resultBooks;
        }

        public async Task<IEnumerable<Books>> GetBooksByGenreAsync(IEnumerable<BookGenres> bookGenres)
        {
            var books = new List<Books>();

            foreach (var bookGenre in bookGenres)
            {
                var book = await _context.Books.FindAsync(bookGenre.BookId);

                if (book != null)
                {
                    books.Add(book);
                }
            }

            var resultBooks = books.Where(b => b.IsVisible).ToList();

            if (resultBooks.Count == 0)
            {
                throw new NotFoundException("No books found.");
            }

            return resultBooks;
        }

        public async Task<IEnumerable<Books>> GetBooksByUserIdAsync(int id)
        {
            var books = await _context.Books.Where(b => b.UserId == id).ToListAsync();

            if (books == null || books.Count == 0)
            {
                throw new NotFoundException($"Books for user with id {id} not found.");
            }

            return books;
        }

        public async Task<Books> UpdateBookAsync(Books book)
        {
            try
            {
                var bookToUpdate = await BookExists(book.Id);

                _context.Entry(bookToUpdate).CurrentValues.SetValues(book);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Book {book.Name} with id {book.Id} is updated");

                return bookToUpdate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                var book = await BookExists(id);

                _context.Books.Remove(book);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Book with id {id} is deleted");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Books> BookExists(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                throw new NotFoundException($"Book with id: {id} not found.");
            }

            return book;
        }
    }
}