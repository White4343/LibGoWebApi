using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using User.API.Data;
using User.API.Data.Entities;
using User.API.Repositories.Interfaces;

namespace User.API.Repositories
{
    public class BoughtBooksRepository : IBoughtBooksRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BoughtBooks> _logger;

        public BoughtBooksRepository(AppDbContext context, ILogger<BoughtBooks> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BoughtBooks> CreateBoughtBookAsync(BoughtBooks boughtBook)
        {
            try
            {
                var result = await _context.BoughtBooks.AddAsync(boughtBook);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Created bought book with id {result.Entity.Id} by user {result.Entity.UserId}");

                return result.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BoughtBooks>> GetBoughtBooksByUserId(int userId)
        {
            try
            {
                var boughtBooks = await _context.BoughtBooks.Where(b => b.UserId == userId).ToListAsync();

                if (boughtBooks == null || boughtBooks.Count == 0)
                {
                    throw new NotFoundException($"No bought books found for user with id {userId}");
                }

                return boughtBooks;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BoughtBooks>> GetBoughtBooksByBookId(int bookId)
        {
            try
            {
                var boughtBooks = await _context.BoughtBooks.Where(b => b.BookId == bookId).ToListAsync();

                if (boughtBooks == null || boughtBooks.Count == 0)
                {
                    throw new NotFoundException($"No bought books found for book with id {bookId}");
                }

                return boughtBooks;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<BoughtBooks> GetBoughtBooksByUserIdByBookId(int userId, int bookId)
        {
            try
            {
                var boughtBook = await _context.BoughtBooks.FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == bookId);

                if (boughtBook == null)
                {
                    throw new NotFoundException($"No bought book found for user with id {userId} and book with id {bookId}");
                }

                return boughtBook;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<BoughtBooks> GetBoughtBooksById(int id)
        {
            try
            {
                var boughtBook = await _context.BoughtBooks.FirstOrDefaultAsync(b => b.Id == id);

                if (boughtBook == null)
                {
                    throw new NotFoundException($"No bought book found with id {id}");
                }

                return boughtBook;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BoughtBooks>> GetBoughtBooks()
        {
            try
            {
                var boughtBooks = await _context.BoughtBooks.ToListAsync();

                if (boughtBooks == null || boughtBooks.Count == 0)
                {
                    throw new NotFoundException("No bought books found");
                }

                return boughtBooks;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteBoughtBooksById(int id)
        {
            try
            {
                var boughtBook = await GetBoughtBooksById(id);

                _context.BoughtBooks.Remove(boughtBook);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteBoughtBooksByUserIdByBookId(int userId, int bookId)
        {
            try
            {
                var boughtBook = await GetBoughtBooksByUserIdByBookId(userId, bookId);

                _context.BoughtBooks.Remove(boughtBook);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteBoughtBooksBySubscriptionId(int subscriptionId)
        {
            try
            {
                var boughtBooks = await _context.BoughtBooks.ToListAsync();

                await Task.WhenAll(boughtBooks.Where(b => b.SubscriptionId == subscriptionId).Select(b => DeleteBoughtBooksById(b.Id)));

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Deleted bought books with subscription id {subscriptionId}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
