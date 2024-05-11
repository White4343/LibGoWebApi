using AutoMapper;
using Book.API.Data.Entities;
using Book.API.Models.Requests.BooksRequests;
using Book.API.Models.Responses.BooksResponses;
using Book.API.Models.Responses.GenresResponses;
using Book.API.Repositories.Interfaces;
using Book.API.Services.Interfaces;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Services
{
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IGenresService _genresService;
        private readonly ILogger<BooksService> _logger;
        private readonly IMapper _mapper;

        public BooksService(IBooksRepository booksRepository, IGenresService genresService, ILogger<BooksService> logger, IMapper mapper)
        {
            _booksRepository = booksRepository;
            _genresService = genresService;
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

        public async Task<GetBookByPageResponse> GetBookPageByIdAsync(int id, int userId)
        {
            try
            {
                var book = await GetBookByIdAsync(id, userId);

                var bookGenres = await _genresService.GetGenresByBookIdAsync(id, null);

                var response = new GetBookByPageResponse
                {
                    Genres = bookGenres.Genres
                };

                if (book.UserId == userId && !book.IsVisible)
                {
                    response.Book = book;
                }
                else
                {
                    throw new UnauthorizedAccessException("You are not the author of this book");
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<GetBooksByGenreResponse> GetGenreBooksPageByIdAsync(int genreId)
        {
            try
            {
                var bookGenresByGenreResponse = await _genresService.GetBooksByGenreIdAsync(genreId, null);

                var booksByGenre = await _booksRepository.GetBooksByGenreAsync(bookGenresByGenreResponse.Books);

                booksByGenre = booksByGenre.Where(b => b.IsVisible).ToList();

                var response = new GetBooksByGenreResponse
                {
                    Genre = bookGenresByGenreResponse.Genre,
                    Books = booksByGenre
                };

                return response;
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

        public async Task<IEnumerable<Books>> GetBooksAsync()
        {
            try
            {
                var books = await _booksRepository.GetBooksAsync();

                return books;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Books> UpdateBookAsync(UpdateBooksRequest book, int userId)
        {
            try
            { 
                await BookExists(book.Id, userId);

                await IsBookAuthor(book.UserId, userId);

                var bookToUpdate = _mapper.Map<Books>(book);

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

                await IsBookAuthor(book.UserId, userId);

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

        private async Task IsBookAuthor(int bookUserId, int userId)
        {
            if (bookUserId == userId)
                return;

            throw new UnauthorizedAccessException("You are not the author of this book");
        }
    }
}