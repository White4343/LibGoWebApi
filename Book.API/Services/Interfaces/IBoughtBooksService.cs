using Book.API.Models.Requests;

namespace Book.API.Services.Interfaces
{
    public interface IBoughtBooksService
    {
        Task GetBoughtBooksByBookId(int bookId, string? token);
        Task GetUserSubscriptionByBookId(int bookId, string? token);
    }
}
