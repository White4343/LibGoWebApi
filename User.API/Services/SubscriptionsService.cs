using SendGrid.Helpers.Errors.Model;
using Stripe.Forwarding;
using User.API.Data.Entities;
using User.API.Models;
using User.API.Models.Requests;
using User.API.Repositories.Interfaces;
using User.API.Services.Interfaces;

namespace User.API.Services
{
    public class SubscriptionsService : ISubscriptionsService
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly ILogger<SubscriptionsService> _logger;
        private readonly IBooksService _booksService;
        private readonly IUserSubscriptionsRepository _userSubscriptionsRepository;

        public SubscriptionsService(ISubscriptionsRepository subscriptionsRepository,
            ILogger<SubscriptionsService> logger, IBooksService booksService)
        {
            _subscriptionsRepository = subscriptionsRepository;
            _logger = logger;
            _booksService = booksService;
        }

        public async Task<Subscriptions> CreateSubscriptionAsync(CreateSubscriptionRequest request, int tokenUserId)
        {
            try
            {
                await IsAuthorHasSubscription(tokenUserId);

                if (request.BookIds.Select(x => x).Distinct().Count() != request.BookIds.Length - 1)
                {
                    throw new BadRequestException($"You can't create Subscription when book list contains duplicates.");
                }

                await IsBooksAuthor(request.BookIds, tokenUserId);

                var subscription = new Subscriptions
                {
                    IsActive = request.IsActive,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    BookIds = request.BookIds,
                    UserId = tokenUserId
                };

                var createdSubscription = await _subscriptionsRepository.CreateSubscriptionAsync(subscription);

                return createdSubscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Subscriptions>> GetSubscriptionsAsync()
        {
            try
            {
                var subscriptions = await _subscriptionsRepository.GetSubscriptionsAsync();

                return subscriptions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Subscriptions> GetSubscriptionByIdAsync(int id)
        {
            try
            {
                var subscription = await _subscriptionsRepository.GetSubscriptionByIdAsync(id);

                return subscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Subscriptions> GetSubscriptionByUserIdAsync(int userId)
        {
            try
            {
                var subscription = await _subscriptionsRepository.GetSubscriptionByUserIdAsync(userId);

                return subscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Subscriptions> GetSubscriptionByBookIdAsync(int bookId)
        {
            try
            {
                var subscription = await _subscriptionsRepository.GetSubscriptionByBookIdAsync(bookId);

                return subscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Subscriptions> UpdateSubscriptionAsync(UpdateSubscriptionRequest request, int tokenUserId)
        {
            try
            {
                var subscription = await GetSubscriptionByIdAsync(request.Id);

                if (!subscription.IsActive)
                {
                    throw new BadRequestException($"You can't update Subscription when it's active.");
                }

                var userSubscriptions = await _userSubscriptionsRepository.GetUserSubscriptionsByAuthorUserIdAsync(subscription.UserId);

                if (userSubscriptions.Any(x => x.IsActive))
                {
                    throw new BadRequestException($"You can't update Subscription when it's active.");
                }

                if (subscription.UserId != tokenUserId)
                {
                    throw new UnauthorizedAccessException($"You can't update Subscription when it's not yours.");
                }

                if (request.BookIds.Select(x => x).Distinct().Count() != request.BookIds.Length - 1)
                {
                    throw new BadRequestException($"You can't update Subscription when book list contains duplicates.");
                }

                await IsBooksAuthor(request.BookIds, tokenUserId);

                subscription.IsActive = request.IsActive;
                subscription.Name = request.Name;
                subscription.Description = request.Description;
                subscription.Price = request.Price;
                subscription.BookIds = request.BookIds.ToList();

                var updatedSubscription = await _subscriptionsRepository.UpdateSubscriptionAsync(subscription);

                return updatedSubscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteSubscriptionAsync(int id, int tokenUserId)
        {
            try
            {
                var subscription = await GetSubscriptionByIdAsync(id);

                if (subscription.IsActive)
                {
                    throw new BadRequestException($"You can't delete Subscription when it's active.");
                }

                var userSubscriptions = await _userSubscriptionsRepository.GetUserSubscriptionsByAuthorUserIdAsync(subscription.UserId);

                if (userSubscriptions.Any(x => x.IsActive))
                {
                    throw new BadRequestException($"You can't update Subscription when it's active.");
                }

                await _subscriptionsRepository.DeleteSubscriptionAsync(subscription);

                return true;
            }
            catch (Exception a)
            {
                Console.WriteLine(a);
                throw;
            }
        }

        public async Task<bool> DeleteSubscriptionByUserIdAsync(int userId, int tokenUserId)
        {
            try
            {
                var subscription = await GetSubscriptionByUserIdAsync(userId);

                if (subscription.IsActive)
                {
                    throw new BadRequestException($"You can't delete Subscription when it's active.");
                }

                // TODO: Check if is it any BoughtSubscription from users before deleting

                await _subscriptionsRepository.DeleteSubscriptionAsync(subscription);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task IsBooksAuthor(IEnumerable<int> bookIds, int tokenUserId)
        {
            try
            {
                foreach (var bookId in bookIds)
                {
                    var book = await _booksService.GetBookByIdAsync(bookId);

                    if (book.UserId != tokenUserId)
                    {
                        throw new UnauthorizedAccessException(
                            $"You can't manipulate Subscription when book in list is not yours.");
                    }

                    if (!book.IsVisible)
                        throw new NotFoundException($"You can't manipulate Subscription when book in list is visible."); }
            }
            catch (NotFoundException e)
            {
                throw new NotFoundException($"You can't manipulate Subscription when book in list is not available.");
            }
        }

        private async Task IsAuthorHasSubscription(int tokenUserId)
        {
            var subscription = await GetSubscriptionByUserIdAsync(tokenUserId);

            if (subscription != null)
            {
                throw new BadRequestException($"You can't create Subscription when you already have one.");
            }
        }
    }
}
