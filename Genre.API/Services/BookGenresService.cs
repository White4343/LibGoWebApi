using Genre.API.Models.Responses;

namespace Genre.API.Services
{
    public class BookGenresService : IBookGenresService
    {
        private readonly IBookGenresRepository _bookGenresRepository;
        private readonly IBookService _bookService;
        private readonly IGenresService _genresService;
        private readonly ILogger<BookGenresService> _logger;
        private readonly IMapper _mapper;

        public BookGenresService(IBookGenresRepository bookGenresRepository, IBookService bookService,
            IGenresService genresService, ILogger<BookGenresService> logger, IMapper mapper)
        {
            _bookGenresRepository = bookGenresRepository;
            _bookService = bookService;
            _genresService = genresService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BookGenres> CreateBookGenreAsync(CreateBookGenresRequest bookGenre, int userId)
        {
            try
            {
                var book = await BookExists(bookGenre.BookId);

                await GenreExists(bookGenre.GenreId);

                await IsBookAuthor(book.UserId, userId);

                var bookGenreToCreate = _mapper.Map<BookGenres>(bookGenre);

                var createdBookGenre = await _bookGenresRepository.CreateBookGenreAsync(bookGenreToCreate);

                return createdBookGenre;
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
                var bookGenre = await _bookGenresRepository.GetBookGenreByIdAsync(id);

                return bookGenre;
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
                var bookGenres = await _bookGenresRepository.GetBookGenresAsync();

                return bookGenres;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<GetGenresByBookIdResponse> GetBookGenresByBookIdAsync(int bookId)
        {
            try
            {
                var bookGenres = await _bookGenresRepository.GetBookGenresByBookIdAsync(bookId);

                var genres = await _genresService.GetGenresAsync();

                // need to get all genres and then filter out the ones that are not in the bookGenres list
                
                genres = genres.Where(g => bookGenres.Any(bg => bg.GenreId == g.Id));

                var result = new GetGenresByBookIdResponse()
                {
                    BookId = bookId,
                    Genres = genres
                };

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<GetBooksByGenreIdResponse> GetBookGenresByGenreIdAsync(int genreId)
        {
            try
            {
                var bookGenres = await _bookGenresRepository.GetBookGenresByGenreIdAsync(genreId);

                var genre = await _genresService.GetGenreByIdAsync(genreId);

                var result = new GetBooksByGenreIdResponse()
                {
                    Genre = genre,
                    Books = bookGenres
                };
                
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<BookGenres> UpdateBookGenreAsync(UpdateBookGenresRequest bookGenre, int userId)
        {
            try
            {
                await BookExists(bookGenre.BookId);

                await GenreExists(bookGenre.GenreId);

                await IsBookAuthor(bookGenre.BookId, userId);

                var bookGenreToUpdate = _mapper.Map<BookGenres>(bookGenre);

                var updatedBookGenre = await _bookGenresRepository.UpdateBookGenreAsync(bookGenreToUpdate);

                return updatedBookGenre;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteBookGenreAsync(int id, int userId)
        {
            try
            {
                var book = await BookExists(id);

                await IsBookAuthor(book.UserId, userId);

                var deleted = await _bookGenresRepository.DeleteBookGenreAsync(id);

                return deleted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteBookGenresByBookIdAsync(int bookId, int userId)
        {
            try
            {
                var book = await BookExists(bookId);

                await IsBookAuthor(book.UserId, userId);

                var deleted = await _bookGenresRepository.DeleteBookGenresByBookIdAsync(bookId);

                return deleted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Books> BookExists(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);

                return book;
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task GenreExists(int id)
        {
            try
            {
                await _genresService.GetGenreByIdAsync(id);
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