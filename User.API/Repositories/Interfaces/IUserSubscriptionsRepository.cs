using User.API.Data.Entities;

namespace User.API.Repositories.Interfaces
{
    public interface IUserSubscriptionsRepository
    {
        Task<UserSubscriptions> CreateUserSubscriptionAsync(UserSubscriptions userSubscription);
        Task<UserSubscriptions> GetUserSubscriptionByIdAsync(int id);
        Task<UserSubscriptions> GetUserSubscriptionByUserIdBookIdAsync(int userId, int bookId);
        Task<UserSubscriptions> GetUserSubscriptionByUserIdSubscriptionIdAsync(int userId, int subscriptionId);
        Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsByUserIdAsync(int userId);
        Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsByAuthorUserIdAsync(int authorUserId);
        Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsAsync();
        Task<UserSubscriptions> PatchUserSubscriptionDateEndAsync(int id);
        Task PatchUserSubscriptionExpiredAsync();
        Task DeleteUserSubscriptionByIdAsync(int id);
    }
}