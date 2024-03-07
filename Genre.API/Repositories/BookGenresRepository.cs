namespace Genre.API.Repositories
{
    public class BookGenresRepository : IBookGenresRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookGenresRepository> _logger;
        private readonly IMapper _mapper;

        public BookGenresRepository(AppDbContext context, ILogger<BookGenresRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<BookGenres> CreateBookGenreAsync(BookGenres bookGenre)
        {
            try
            {
                var bookGenreToCreate = await _context.BookGenres.AddAsync(bookGenre);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"BookGenre with id {bookGenreToCreate.Entity.Id} genre id {bookGenre.GenreId} with id {bookGenre.BookId} is created");

                return bookGenreToCreate.Entity;
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
                var bookGenre = await BookGenreExists(id);

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
            var bookGenres = await _context.BookGenres.ToListAsync();

            if (bookGenres == null)
            {
                throw new NotFoundException("No book genres found.");
            }

            return bookGenres;
        }

        public async Task<IEnumerable<BookGenres>> GetBookGenresByBookIdAsync(int bookId)
        {
            var bookGenres = await _context.BookGenres.Where(bg => bg.BookId == bookId).ToListAsync();

            if (bookGenres == null)
            {
                throw new NotFoundException($"No book genres found by id {bookId}.");
            }

            return bookGenres;
        }

        public async Task<IEnumerable<BookGenres>> GetBookGenresByGenreIdAsync(int genreId)
        {
            var bookGenres = await _context.BookGenres.Where(bg => bg.GenreId == genreId).ToListAsync();

            if (bookGenres == null)
            {
                throw new NotFoundException($"No book genres found by id {genreId}.");
            }

            return bookGenres;
        }

        public async Task<BookGenres> UpdateBookGenreAsync(BookGenres bookGenre)
        {
            try
            {
                var bookGenreToUpdate = await BookGenreExists(bookGenre.Id);

                _context.BookGenres.Update(bookGenre);

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    $"BookGenre with id {bookGenreToUpdate.Id} genre id {bookGenre.GenreId} with id {bookGenre.BookId} is updated");

                return bookGenreToUpdate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteBookGenreAsync(int id)
        {
            try
            {
                var bookGenre = await BookGenreExists(id);

                _context.BookGenres.Remove(bookGenre);

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    $"BookGenre with id {bookGenre.Id} genre id {bookGenre.GenreId} with id {bookGenre.BookId} is deleted");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteBookGenresByBookIdAsync(int bookId)
        {
            try
            {
                var bookGenres = await _context.BookGenres.Where(bg => bg.BookId == bookId).ToListAsync();

                if (bookGenres == null)
                {
                    throw new NotFoundException("No book genres found.");
                }

                _context.BookGenres.RemoveRange(bookGenres);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"BookGenres with book id {bookId} are deleted");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<BookGenres> BookGenreExists(int id)
        {
            var bookGenre = await _context.BookGenres.FindAsync(id);

            if (bookGenre == null)
            {
                throw new NotFoundException("BookGenre not found");
            }

            return bookGenre;
        }
    }
}