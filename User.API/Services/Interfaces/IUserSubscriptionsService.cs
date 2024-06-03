using User.API.Data.Entities;
using User.API.Models.Requests.UserSubscriptionRequests;

namespace User.API.Services.Interfaces
{
    public interface IUserSubscriptionsService
    {
        Task<UserSubscriptions> CreateUserSubscriptionAsync(CreateUserSubscriptionRequest createUserSubscriptionRequest);
        Task<UserSubscriptions> GetUserSubscriptionByIdAsync(int id, int tokenUserId);
        Task<UserSubscriptions> GetUserSubscriptionByUserIdBookIdAsync(int userId, int bookId, int tokenUserId);
        Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsByUserIdAsync(int userId, int tokenUserId);
        Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsByAuthorUserIdAsync(int authorUserId, int tokenUserId);
        Task<UserSubscriptions> GetUserSubscriptionByUserIdSubscriptionIdAsync(int userId, int subscriptionId);
        Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsAsync();
        Task<UserSubscriptions> PatchUserSubscriptionDateEndAsync(int id);
        Task PatchUserSubscriptionExpiredAsync();
        Task DeleteUserSubscriptionByIdAsync(int id);
    }
}