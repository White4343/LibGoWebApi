using User.API.Data.Entities;
using User.API.Models.Requests;

namespace User.API.Services.Interfaces
{
    public interface ISubscriptionsService
    {
        Task<Subscriptions> CreateSubscriptionAsync(CreateSubscriptionRequest request, int tokenUserId);
        Task<IEnumerable<Subscriptions>> GetSubscriptionsAsync();
        Task<Subscriptions> GetSubscriptionByIdAsync(int id);
        Task<Subscriptions> GetSubscriptionByUserIdAsync(int userId);
        Task<Subscriptions> GetSubscriptionByBookIdAsync(int bookId);
        Task<Subscriptions> UpdateSubscriptionAsync(UpdateSubscriptionRequest request, int tokenUserId);
        Task<bool> DeleteSubscriptionAsync(int id, int tokenUserId);
        Task<bool> DeleteSubscriptionByUserIdAsync(int userId, int tokenUserId);
    }
}
