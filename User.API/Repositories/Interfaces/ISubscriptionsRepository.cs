using User.API.Data.Entities;

namespace User.API.Repositories.Interfaces
{
    public interface ISubscriptionsRepository
    {
        Task<Subscriptions> CreateSubscriptionAsync(Subscriptions subscription);
        Task<IEnumerable<Subscriptions>> GetSubscriptionsAsync();
        Task<Subscriptions> GetSubscriptionByIdAsync(int id);
        Task<Subscriptions> GetSubscriptionByUserIdAsync(int userId);
        Task<Subscriptions> GetSubscriptionByBookIdAsync(int bookId);
        Task<Subscriptions> UpdateSubscriptionAsync(Subscriptions subscription);
        Task<Subscriptions> DeleteSubscriptionAsync(Subscriptions subscription);
    }
}