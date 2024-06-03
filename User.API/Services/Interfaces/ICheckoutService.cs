using Stripe.Checkout;

namespace User.API.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<string> CreateBookCheckoutSessionAsync(int bookId, int tokenUserId, string apiUrl, string clientUrl);
        Task<string> CreateSubscriptionCheckoutSessionAsync(int subId, int tokenUserId, string apiUrl, string clientUrl);
        Task SuccessfulBookCheckoutSessionAsync(int bookId, int userId);
        Task SuccessfulSubCheckoutSessionAsync(int subId, int userId);
    }
}
