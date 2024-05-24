using User.API.Data.Entities;
using User.API.Models;
using User.API.Models.Responses;

namespace User.API.Services.Interfaces
{
    public interface IBoughtBooksService
    {
        Task<BoughtBooks> CreateBoughtBookAsync(int bookId, int userId);
        Task<IEnumerable<GetBoughtBooksClientResponse>> GetBoughtBooksByUserId(int userId, int tokenUseId);
        Task<IEnumerable<BoughtBooks>> GetBoughtBooksByBookId(int bookId, int tokenUserId);
        Task<GetBoughtBooksClientResponse> GetBoughtBooksByUserIdByBookId(int userId, int bookId, int tokenUserId);
        Task<BoughtBooks> GetBoughtBooksById(int id);
        Task<IEnumerable<BoughtBooks>> GetBoughtBooks();
        Task DeleteBoughtBooksById(int id);
        Task DeleteBoughtBooksByUserIdByBookId(int userId, int bookId);
        Task DeleteBoughtBooksBySubscriptionId(int subscriptionId);
    }
}