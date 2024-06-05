using SendGrid.Helpers.Errors.Model;
using Stripe.Checkout;
using User.API.Models.Requests.UserSubscriptionRequests;
using User.API.Services.Interfaces;

namespace User.API.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IBooksService _booksService;
        private readonly IBoughtBooksService _boughtBooksService;
        private readonly ISubscriptionsService _subscriptionsService;
        private readonly IUserSubscriptionsService _userSubscriptionsService;

        public CheckoutService(IBooksService booksService, IBoughtBooksService boughtBooksService, 
            ISubscriptionsService subscriptionsService, IUserSubscriptionsService userSubscriptionsService)
        {
            _booksService = booksService;
            _boughtBooksService = boughtBooksService;
            _subscriptionsService = subscriptionsService;
            _userSubscriptionsService = userSubscriptionsService;
        }


        public async Task<string> CreateBookCheckoutSessionAsync(int bookId, int tokenUserId, string apiUrl, string clientUrl)
        {
            var book = await _booksService.GetBookByIdAsync(bookId);

            if (!book.IsAvailableToBuy)
            {
                throw new BadRequestException("Book is not available to buy.");
            }

            if (book.Price == 0)
            {
                throw new BadRequestException("Book is free.");
            }

            var options = new SessionCreateOptions
            {
                // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                SuccessUrl = $"{apiUrl}/checkout/success?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                CancelUrl = clientUrl + "failed",  // Checkout cancelled.
                PaymentMethodTypes = new List<string> // Only card available in test mode?
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = book.Price, // Price is in USD cents.
                            Currency = "UAH",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Metadata = new Dictionary<string, string>
                                {
                                    {"bookId", bookId.ToString()},
                                    {"userId", tokenUserId.ToString()}
                                },
                                Name = book.Name,
                                Description = book.Description,
                                Images = new List<string> { book.PhotoUrl }
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Id;
        }

        public async Task<string> CreateSubscriptionCheckoutSessionAsync(int subId, int tokenUserId, string apiUrl, string clientUrl)
        {
            var subscription = await _subscriptionsService.GetSubscriptionByIdAsync(subId);

            if (!subscription.IsActive)
            {
                throw new BadRequestException("Subscription is not active.");
            }

            var userSubscription =
                await _userSubscriptionsService.GetUserSubscriptionByUserIdSubscriptionIdAsync(tokenUserId, subId);

            if (userSubscription != null || userSubscription.IsActive && userSubscription.EndDate > DateTime.UtcNow)
            {
                throw new BadRequestException("User already has this subscription.");
            }

            var options = new SessionCreateOptions
            {
                // Stripe calls the URLs below when certain checkout events happen such as success and failure.
                SuccessUrl = $"{apiUrl}/checkout/success?sessionId=" + "{CHECKOUT_SESSION_ID}", // Customer paid.
                CancelUrl = clientUrl + "failed",  // Checkout cancelled.
                PaymentMethodTypes = new List<string> // Only card available in test mode?
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = subscription.Price, // Price is in USD cents.
                            Currency = "UAH",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Metadata = new Dictionary<string, string>
                                {
                                    {"subscriptionId", subscription.Id.ToString()},
                                    {"userId", tokenUserId.ToString()}
                                },
                                Name = subscription.Name,
                                Description = subscription.Description,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Id;
        }

        public async Task SuccessfulBookCheckoutSessionAsync(int bookId, int userId)
        {
            try
            {
                await _boughtBooksService.CreateBoughtBookAsync(bookId, userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SuccessfulSubCheckoutSessionAsync(int subId, int userId)
        {
            try
            {
                var userSubscription =
                    await _userSubscriptionsService.GetUserSubscriptionByUserIdSubscriptionIdAsync(userId, subId);

                if (userSubscription != null)
                {
                    await _userSubscriptionsService.PatchUserSubscriptionDateEndAsync(userSubscription.Id);
                }

                CreateUserSubscriptionRequest request = new CreateUserSubscriptionRequest
                {
                    SubscriptionId = subId,
                    UserId = userId
                };

                await _userSubscriptionsService.CreateUserSubscriptionAsync(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
