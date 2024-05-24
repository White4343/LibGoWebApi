using Stripe.Checkout;

namespace User.API.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<string> CreateCheckoutSessionAsync(int bookId, int tokenUserId, string apiUrl, string clientUrl);
        Task SuccessfulCheckoutSessionAsync(int bookId, int userId);
    }
}
