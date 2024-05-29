using Stripe.Checkout;
using User.API.Services.Interfaces;

namespace User.API.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IBooksService _booksService;
        private readonly IBoughtBooksService _boughtBooksService;

        public CheckoutService(IBooksService booksService, IBoughtBooksService boughtBooksService)
        {
            _booksService = booksService;
            _boughtBooksService = boughtBooksService;
        }


        public async Task<string> CreateCheckoutSessionAsync(int bookId, int tokenUserId, string apiUrl, string clientUrl)
        {
            var book = await _booksService.GetBookByIdAsync(bookId);

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

        public async Task SuccessfulCheckoutSessionAsync(int bookId, int userId)
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
    }
}
