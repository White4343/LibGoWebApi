using AutoMapper;
using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests.BooksRequests;
using Book.API.Models.Responses.BooksResponses;
using Book.API.Models.Responses.GenresResponses;
using Book.API.Repositories.Interfaces;
using Book.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using SendGrid.Helpers.Errors.Model;
using StackExchange.Redis;

namespace Book.API.Services
{
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;
        private readonly ILogger<BooksService> _logger;
        private readonly IMapper _mapper;
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IUsersService _usersService;
        private readonly IBoughtBooksService _boughtBooksService;

        public BooksService(IBooksRepository booksRepository, ILogger<BooksService> logger, IMapper mapper,
            IConnectionMultiplexer redisConnection, IUsersService usersService, IBoughtBooksService boughtBooksService)
        {
            _booksRepository = booksRepository;
            _logger = logger;
            _mapper = mapper;
            _redisConnection = redisConnection;
            _usersService = usersService;
            _boughtBooksService = boughtBooksService;
        }

        public async Task<Books> CreateBookAsync(CreateBooksRequest book, int userId)
        {
            try
            {
                CheckPrice(book.Price);

                await UsersExists(book.CoAuthorIds);

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
                var db = _redisConnection.GetDatabase();
                var cachedBooks = await db.StringGetAsync("books");

                if (!cachedBooks.IsNullOrEmpty)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<Books>>(cachedBooks);
                }

                var books = await _booksRepository.GetBooksAsync();

                books = books.Where(b => b.IsVisible).ToList();

                await db.StringSetAsync("books", JsonConvert.SerializeObject(books), TimeSpan.FromMinutes(300));

                return books;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Books>> GetBooksByBookNameAsync(string name)
        {
            try
            {
                var books = await _booksRepository.GetBooksByBookNameAsync(name);

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

                await UsersExists(book.CoAuthorIds);

                var val = int.TryParse(bookRequest.Price.ToString(), out var price);

                CheckPrice(price);

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

        // TODO: If book is bought once or more, then just delete userId, disable book from buying
        public async Task<bool> DeleteBookAsync(int id, int userId, string token)
        {
            try
            {
                var book = await BookExists(id, userId);

                IsBookAuthor(book.UserId, userId);

                await _boughtBooksService.GetUserSubscriptionByBookId(id, token);

                await _boughtBooksService.GetBoughtBooksByBookId(id, token);

                var deleted = await _booksRepository.DeleteBookAsync(id);

                return deleted;
            }
            catch (UnauthorizedException e)
            {
                throw new UnauthorizedAccessException(e.Message);
            }
            catch (UnsupportedContentTypeException e)
            {
                var book = await BookExists(id, userId);

                book.IsAvailableToBuy = false;
                book.UserId = 0;

                await _booksRepository.UpdateBookAsync(book);
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

        private void CheckPrice(int price)
        {
            if (price < 100 || price > 500)
            {
                throw new FormatException("Price is too low or too high. Price must be between 100 and 500");
            }
        }

        private async Task UsersExists(int[] userIds)
        {
            try
            {
                if (userIds.Distinct().Count() != userIds.Length)
                {
                    throw new FormatException("CoAuthors must be unique");
                }

                foreach (var id in userIds)
                {
                    await _usersService.GetUserByIdAsync(id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}