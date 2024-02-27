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
                await BookExists(bookGenre.BookId);

                await GenreExists(bookGenre.GenreId);

                await IsBookAuthor(bookGenre.BookId, userId);

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
                await IsBookAuthor(id, userId);

                var deleted = await _bookGenresRepository.DeleteBookGenreAsync(id);

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

        private async Task IsBookAuthor(int bookId, int userId)
        {
            if (bookId == userId) 
                return;
            
            throw new UnauthorizedAccessException("You are not the author of this book");
        }
    }
}