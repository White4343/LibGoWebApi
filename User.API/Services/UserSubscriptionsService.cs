using SendGrid.Helpers.Errors.Model;
using Stripe;
using User.API.Data.Entities;
using User.API.Models.Requests.UserSubscriptionRequests;
using User.API.Repositories.Interfaces;
using User.API.Services.Interfaces;

namespace User.API.Services
{
    public class UserSubscriptionsService : IUserSubscriptionsService
    {
        private readonly IUserSubscriptionsRepository _userSubscriptionsRepository;
        private readonly ILogger<UserSubscriptionsService> _logger;
        private readonly ISubscriptionsService _subscriptionsService;

        public UserSubscriptionsService(IUserSubscriptionsRepository userSubscriptionsRepository,
            ILogger<UserSubscriptionsService> logger, ISubscriptionsService subscriptionsService)
        {
            _userSubscriptionsRepository = userSubscriptionsRepository;
            _logger = logger;
            _subscriptionsService = subscriptionsService;
        }


        public async Task<UserSubscriptions> CreateUserSubscriptionAsync(CreateUserSubscriptionRequest createUserSubscriptionRequest)
        {
            try
            {
                var subscription = await _subscriptionsService.GetSubscriptionByIdAsync(createUserSubscriptionRequest.SubscriptionId);

                if (!subscription.IsActive)
                {
                    throw new BadRequestException("Subscription is not available to be bought.");
                }

                var existingSubscription =
                    await GetUserSubscriptionByUserIdSubscriptionIdAsync(createUserSubscriptionRequest.UserId,
                        createUserSubscriptionRequest.SubscriptionId);

                if (existingSubscription != null)
                {
                    throw new BadRequestException("You already have subscription to this author.");
                }

                var userSubscription = new UserSubscriptions
                {
                    IsActive = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(1),
                    PaidPrice = subscription.Price,
                    IsPaidToAuthor = false,
                    SubscriptionId = createUserSubscriptionRequest.SubscriptionId,
                    UserId = createUserSubscriptionRequest.UserId,
                    AuthorUserId = subscription.UserId,
                    BookIds = subscription.BookIds
                };

                return await _userSubscriptionsRepository.CreateUserSubscriptionAsync(userSubscription);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserSubscriptions> GetUserSubscriptionByIdAsync(int id, int tokenUserId)
        {
            try
            {
                var result = await _userSubscriptionsRepository.GetUserSubscriptionByIdAsync(id);

                if (result.UserId != tokenUserId || result.AuthorUserId != tokenUserId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this subscription.");
                }

                result = await PatchUserSubscriptionIsActiveFalseAsync(id);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserSubscriptions> GetUserSubscriptionByUserIdBookIdAsync(int userId, int bookId, int tokenUserId)
        {
            try
            {
                var userSubscription = await _userSubscriptionsRepository.GetUserSubscriptionByUserIdBookIdAsync(userId, bookId);

                if (userSubscription.UserId != tokenUserId || userSubscription.AuthorUserId != tokenUserId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this subscription.");
                }

                var result = await PatchUserSubscriptionIsActiveFalseAsync(userSubscription.Id);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsByUserIdAsync(int userId, int tokenUserId)
        {
            try
            {
                var result = await _userSubscriptionsRepository.GetUserSubscriptionsByUserIdAsync(userId);

                if (result.Any(x => x.UserId != tokenUserId))
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this subscription.");
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsByAuthorUserIdAsync(int authorUserId, int tokenUserId)
        {
            try
            {
                var result = await _userSubscriptionsRepository.GetUserSubscriptionsByAuthorUserIdAsync(authorUserId);

                if (result.Any(x => x.AuthorUserId != tokenUserId))
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this subscription.");
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserSubscriptions> GetUserSubscriptionByUserIdSubscriptionIdAsync(int userId, int subscriptionId)
        {
            try
            {
                var existingSubscription = await _userSubscriptionsRepository.GetUserSubscriptionByUserIdSubscriptionIdAsync(userId, subscriptionId);

                existingSubscription = await PatchUserSubscriptionIsActiveFalseAsync(existingSubscription.Id);

                return existingSubscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsAsync()
        {
            try
            {
                return await _userSubscriptionsRepository.GetUserSubscriptionsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsByBookIdAsync(int bookId, int tokenUserId)
        {
            try
            {
                var result = await _userSubscriptionsRepository.GetUserSubscriptionsByBookIdAsync(bookId);

                if (result.Any(x => x.UserId != tokenUserId))
                {
                    throw new UnauthorizedAccessException("You are not authorized to view this subscriptions.");
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserSubscriptions> PatchUserSubscriptionDateEndAsync(int id)
        {
            try
            {
                var userSubscription = await _userSubscriptionsRepository.GetUserSubscriptionByIdAsync(id);

                var subscription = await _subscriptionsService.GetSubscriptionByIdAsync(userSubscription.SubscriptionId);

                if (!subscription.IsActive)
                {
                    throw new BadRequestException("Subscription is not available to be bought.");
                }

                if (userSubscription.EndDate != DateTime.UtcNow || userSubscription.IsActive)
                {
                    throw new BadRequestException($"Subscription is not available to be bought now at {DateTime.UtcNow}. You can buy it at {userSubscription.EndDate}");
                }

                return await _userSubscriptionsRepository.PatchUserSubscriptionDateEndAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserSubscriptions> PatchUserSubscriptionIsActiveFalseAsync(int id)
        {
            try
            {
                var userSubscription = await _userSubscriptionsRepository.GetUserSubscriptionByIdAsync(id);

                if (!userSubscription.IsActive)
                {
                    return userSubscription;
                }

                if (userSubscription.EndDate > DateTime.UtcNow && userSubscription.IsActive)
                {
                    return await _userSubscriptionsRepository.PatchUserSubscriptionIsActiveFalseAsync(id);
                }

                return userSubscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task PatchUserSubscriptionExpiredAsync()
        {
            try
            {
                await _userSubscriptionsRepository.PatchUserSubscriptionExpiredAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteUserSubscriptionByIdAsync(int id)
        {
            try
            {
                await _userSubscriptionsRepository.DeleteUserSubscriptionByIdAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}