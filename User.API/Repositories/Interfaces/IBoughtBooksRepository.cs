using User.API.Data.Entities;

namespace User.API.Repositories.Interfaces
{
    public interface IBoughtBooksRepository
    {
        Task<BoughtBooks> CreateBoughtBookAsync(BoughtBooks boughtBook);
        Task<IEnumerable<BoughtBooks>> GetBoughtBooksByUserId(int userId);
        Task<IEnumerable<BoughtBooks>> GetBoughtBooksByBookId(int bookId);
        Task<BoughtBooks> GetBoughtBooksByUserIdByBookId(int userId, int bookId);
        Task<BoughtBooks> GetBoughtBooksById(int id);
        Task<IEnumerable<BoughtBooks>> GetBoughtBooks();
        Task DeleteBoughtBooksById(int id);
        Task DeleteBoughtBooksByUserIdByBookId(int userId, int bookId);
        Task DeleteBoughtBooksBySubscriptionId(int subscriptionId);
    }
}