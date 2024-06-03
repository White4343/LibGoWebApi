using AutoMapper;
using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests.BooksRequests;
using Book.API.Models.Responses.BooksResponses;
using Book.API.Models.Responses.GenresResponses;
using Book.API.Repositories.Interfaces;
using Book.API.Services.Interfaces;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Services
{
    // TODO: Patch Requests?
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;
        private readonly ILogger<BooksService> _logger;
        private readonly IMapper _mapper;

        public BooksService(IBooksRepository booksRepository, ILogger<BooksService> logger, IMapper mapper)
        {
            _booksRepository = booksRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Books> CreateBookAsync(CreateBooksRequest book, int userId)
        {
            try
            {
                var bookToCreate = new Books
                {
                    Name = book.Name,
                    Description = book.Description,
                    Price = book.Price,
                    PublishDate = DateTime.UtcNow,
                    PhotoUrl = book.PhotoUrl,
                    IsVisible = book.IsVisible,
                    UserId = userId,
                    CoAuthorIds = book.CoAuthorIds
                };

                var createdBook = await _booksRepository.CreateBookAsync(bookToCreate);

                return createdBook;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Books> GetBookByIdAsync(int id, int userId)
        {
            try
            {
                var book = await _booksRepository.GetBookByIdAsync(id);

                if (book.UserId == userId)
                {
                    return book;
                }

                if (!book.IsVisible)
                {
                    throw new UnauthorizedAccessException("You are not the author of this book");
                }

                return book;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Books>> GetBooksByUserIdAsync(int id, int userId)
        {
            try
            {
                var books = await _booksRepository.GetBooksByUserIdAsync(id);

                var firstBook = books.FirstOrDefault();

                if (firstBook.UserId == userId)
                {
                    return books;
                }

                books = books.Where(b => b.IsVisible).ToList();

                return books;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Books>> GetBooksByGenreAsync(IEnumerable<BookGenresDto> bookGenres)
        {
            try
            {
                var result = await _booksRepository.GetBooksByGenreAsync(bookGenres);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Books>> GetBooksAsync()
        {
            try
            {
                var books = await _booksRepository.GetBooksAsync();

                books = books.Where(b => b.IsVisible).ToList();

                return books;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Books> UpdateBookAsync(UpdateBooksRequest bookRequest, int userId)
        {
            try
            { 
                var book = await BookExists(bookRequest.Id, userId);

                IsBookAuthor(book.UserId, userId);

                var bookToUpdate = _mapper.Map<Books>(bookRequest);

                bookToUpdate.UserId = userId;

                await _booksRepository.UpdateBookAsync(bookToUpdate);

                return bookToUpdate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int id, int userId)
        {
            try
            {
                var book = await BookExists(id, userId);

                IsBookAuthor(book.UserId, userId);

                var deleted = await _booksRepository.DeleteBookAsync(id);

                return deleted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<Books> BookExists(int id, int userId)
        {
            try
            {
                var book = await GetBookByIdAsync(id, userId);

                return book;
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void IsBookAuthor(int bookUserId, int userId)
        {
            if (bookUserId == userId)
                return;

            throw new UnauthorizedAccessException("You are not the author of this book");
        }
    }
}