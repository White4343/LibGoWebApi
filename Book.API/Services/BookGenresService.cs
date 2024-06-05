using AutoMapper;
using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests.BookGenresRequests;
using Book.API.Models.Responses.BooksResponses;
using Book.API.Models.Responses.GenresResponses;
using Book.API.Repositories;
using Book.API.Repositories.Interfaces;
using Book.API.Services.Interfaces;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Services
{
    // TODO: Maybe add userId to BookGenres Entity?
    public class BookGenresService : IBookGenresService
    {
        private readonly IBookGenresRepository _bookGenresRepository;
        private readonly IBooksService _booksService;
        private readonly IGenresService _genresService;
        private readonly ILogger<BookGenresService> _logger;
        private readonly IMapper _mapper;

        public BookGenresService(IBookGenresRepository bookGenresRepository, 
            IBooksService booksService, ILogger<BookGenresService> logger, IMapper mapper, IGenresService genresService)
        {
            _bookGenresRepository = bookGenresRepository;
            _booksService = booksService;
            _logger = logger;
            _mapper = mapper;
            _genresService = genresService;
        }


        public async Task<BookGenresDto> CreateBookGenreAsync(CreateBookGenresRequests bookGenre, int tokenUserId)
        {
            try
            {
                var bookGenreRequest = new BookGenres
                {
                    BookId = bookGenre.BookId,
                    GenreId = bookGenre.GenreId
                };

                await _booksService.GetBookByIdAsync(bookGenre.BookId, tokenUserId);

                var createdBookGenre = await _bookGenresRepository.CreateBookGenreAsync(bookGenreRequest);

                var result = _mapper.Map<BookGenresDto>(createdBookGenre);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<BookGenresDto> GetBookGenreByIdAsync(int id, int tokenUserId)
        {
            try
            {
                var bookGenre = await _bookGenresRepository.GetBookGenreByIdAsync(id);

                await _booksService.GetBookByIdAsync(bookGenre.BookId, tokenUserId);

                var result = _mapper.Map<BookGenresDto>(bookGenre);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BookGenresDto>> GetBookGenresByGenreIdAsync(int genreId, int tokenUserId)
        {
            try
            {
                var bookGenres = await _bookGenresRepository.GetBookGenresByGenreIdAsync(genreId);

                foreach (var bookGenre in bookGenres)
                {
                    try
                    {
                        await _booksService.GetBookByIdAsync(bookGenre.BookId, tokenUserId);
                    }
                    catch (Exception e)
                    {
                        bookGenres = bookGenres.SkipWhile(bg => bg.BookId == bookGenre.BookId);

                        continue;
                    }
                }

                var result = _mapper.Map<IEnumerable<BookGenresDto>>(bookGenres);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BookGenresDto>> GetBookGenresByBookIdAsync(int bookId, int tokenUserId)
        {
            try
            {
                var bookGenres = await _bookGenresRepository.GetBookGenresByBookIdAsync(bookId);

                foreach (var bookGenre in bookGenres)
                {
                    try
                    {
                        await _booksService.GetBookByIdAsync(bookGenre.BookId, tokenUserId);
                    }
                    catch (Exception e)
                    {
                        bookGenres = bookGenres.SkipWhile(bg => bg.BookId == bookGenre.BookId);

                        continue;
                    }
                }

                var result = _mapper.Map<IEnumerable<BookGenresDto>>(bookGenres);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<GetBooksWithGenreNamesResponse>> GetAllBooksWithGenreNamesAsync()
        {
            try
            {
                var books = await _booksService.GetBooksAsync();

                var result = await GetGenreNamesByBooks(books);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<GetBooksWithGenreNamesResponse>> GetBooksByBookNameWithGenreNamesAsync(string name)
        {
            try
            {
                var books = await _booksService.GetBooksByBookNameAsync(name);

                var result = await GetGenreNamesByBooks(books);

                return result;
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
                var book = await _booksService.GetBookByIdAsync(id, userId);

                var bookGenres = await GetBookGenresWithGenreNamesByBookId(id, userId);

                var response = new GetBookByPageResponse
                {
                    Genres = bookGenres.Genres
                };

                if (book.UserId != userId && !book.IsVisible)
                {
                    throw new UnauthorizedAccessException("You are not the author of this book");
                }
                else
                {
                    response.Book = book;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<GetBooksByGenreResponse> GetGenreBooksPageByIdAsync(int genreId, int tokenUserId)
        {
            try
            {
                var bookGenresByGenreResponse = await GetBookGenresWithGenreNameByGenreId(genreId, tokenUserId);

                var booksByGenre = await _booksService.GetBooksByGenreAsync(bookGenresByGenreResponse.Books);

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

        public async Task<BookGenresDto> UpdateBookGenreAsync(BookGenres bookGenre, int tokenUserId)
        {
            try
            {
                var book = await _booksService.GetBookByIdAsync(bookGenre.BookId, tokenUserId);

                IsBookAuthor(book.UserId, tokenUserId);

                var updatedBookGenre = await _bookGenresRepository.UpdateBookGenreAsync(bookGenre);

                var result = _mapper.Map<BookGenresDto>(updatedBookGenre);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteBookGenreAsync(int id, int tokenUserId)
        {
            try
            {
                var bookGenre = await _bookGenresRepository.GetBookGenreByIdAsync(id);

                var book = await _booksService.GetBookByIdAsync(bookGenre.BookId, tokenUserId);

                IsBookAuthor(book.UserId, tokenUserId);

                await _bookGenresRepository.DeleteBookGenreAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void IsBookAuthor(int bookUserId, int tokenUserId)
        {
            if (bookUserId != tokenUserId)
            {
                throw new UnauthorizedAccessException("You are not the author of this book");
            }
        }

        private async Task<GetGenresByBookIdResponse> GetBookGenresWithGenreNamesByBookId(int bookId, int userId)
        {
            try
            {
                var bookGenres = await GetBookGenresByBookIdAsync(bookId, userId);

                var genres = new List<Genres>();

                foreach (var bookGenre in bookGenres)
                {
                    var genre = await _genresService.GetGenreByIdAsync(bookGenre.GenreId);

                    genres.Add(genre);
                }

                var bookGenresResponse = new GetGenresByBookIdResponse
                {
                    BookId = bookId,
                    Genres = genres
                };

                return bookGenresResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<GetBooksByGenreIdResponse> GetBookGenresWithGenreNameByGenreId(int genreId, int userId)
        {
            try
            {
                var bookGenres = await GetBookGenresByGenreIdAsync(genreId, userId);

                var genre = await _genresService.GetGenreByIdAsync(genreId);

                var bookGenresResponse = new GetBooksByGenreIdResponse
                {
                    Genre = genre,
                    Books = bookGenres
                };

                return bookGenresResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<IEnumerable<GetBooksWithGenreNamesResponse>> GetGenreNamesByBooks(IEnumerable<Books> books)
        {
            try
            {
                var result = new List<GetBooksWithGenreNamesResponse>();

                foreach (var book in books)
                {
                    try
                    {
                        var bookGenre = await GetBookGenresWithGenreNamesByBookId(book.Id, book.UserId);

                        var item = new GetBooksWithGenreNamesResponse
                        {
                            Book = book,
                            Genres = bookGenre.Genres
                        };

                        result.Add(item);
                    }
                    catch (NotFoundException e)
                    {
                        var item = new GetBooksWithGenreNamesResponse
                        {
                            Book = book,
                            Genres = null
                        };

                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
