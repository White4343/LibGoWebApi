using AutoMapper;
using User.API.Data.Entities;
using User.API.Models;
using User.API.Models.Requests;
using User.API.Models.Responses;
using User.API.Repositories.Interfaces;
using User.API.Services.Interfaces;

namespace User.API.Services
{
    public class BoughtBooksService : IBoughtBooksService
    {
        private readonly IBoughtBooksRepository _boughtBooksRepository;
        private readonly IBooksService _booksService;
        private readonly IMapper _mapper;

        public BoughtBooksService(IBoughtBooksRepository boughtBooksRepository, IBooksService booksService, IMapper mapper)
        {
            _boughtBooksRepository = boughtBooksRepository;
            _booksService = booksService;
            _mapper = mapper;
        }


        public async Task<BoughtBooks> CreateBoughtBookAsync(int bookId, int userId)
        {
            try
            {
                var book = await BookExists(bookId);

                var BoughtBook = new BoughtBooks
                {
                    PurchaseDate = DateTime.UtcNow,
                    Price = book.Price,
                    IsPaidToAuthor = false,
                    AuthorUserId = book.UserId,
                    UserId = userId,
                    BookId = book.Id
                };

                var result = await _boughtBooksRepository.CreateBoughtBookAsync(BoughtBook);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<GetBoughtBooksClientResponse>> GetBoughtBooksByUserId(int userId, int tokenUseId)
        {
            try
            {
                var boughtBooks = await _boughtBooksRepository.GetBoughtBooksByUserId(userId);

                if (boughtBooks.Any(b => b.UserId != tokenUseId))
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this user's bought books");
                }

                var response = _mapper.Map<IEnumerable<GetBoughtBooksClientResponse>>(boughtBooks);

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BoughtBooks>> GetBoughtBooksByBookId(int bookId, int tokenUserId)
        {
            try
            {
                var boughtBooks = await _boughtBooksRepository.GetBoughtBooksByBookId(bookId);

                if (boughtBooks.Any(bb => bb.AuthorUserId != tokenUserId))
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this book's bought books");
                }

                return boughtBooks;
            }
            catch (Exception a)
            {
                Console.WriteLine(a);
                throw;
            }
        }

        public async Task<GetBoughtBooksClientResponse> GetBoughtBooksByUserIdByBookId(int userId, int bookId, int tokenUserId)
        {
            try
            {
                var boughtBook = await _boughtBooksRepository.GetBoughtBooksByUserIdByBookId(userId, bookId);

                if (boughtBook.UserId != tokenUserId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this user's bought book");
                }

                var response = _mapper.Map<GetBoughtBooksClientResponse>(boughtBook);

                return response;
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
                var boughtBook = await _boughtBooksRepository.GetBoughtBooksById(id);

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
                var boughtBooks = await _boughtBooksRepository.GetBoughtBooks();

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
                await _boughtBooksRepository.DeleteBoughtBooksById(id);
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
                await _boughtBooksRepository.DeleteBoughtBooksByUserIdByBookId(userId, bookId);
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
                await _boughtBooksRepository.DeleteBoughtBooksBySubscriptionId(subscriptionId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Books> BookExists(int bookId)
        {
            try
            {
                var book = await _booksService.GetBookByIdAsync(bookId);

                return book;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
