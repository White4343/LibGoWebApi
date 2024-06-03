using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using User.API.Models.Responses;
using Stripe.Checkout;
using User.API.Services.Interfaces;
using System.Security.Claims;
using Stripe;

namespace User.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CheckoutController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICheckoutService _checkoutService;
        private readonly ILogger<CheckoutController> _logger;

        public static string s_wasmClientURL = string.Empty;

        public CheckoutController(IConfiguration configuration, ICheckoutService checkoutService, ILogger<CheckoutController> logger)
        {
            _configuration = configuration;
            _checkoutService = checkoutService;
            _logger = logger;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> CheckoutOrder(int id, [FromServices] IServiceProvider sp, [FromBody] bool? isSubscription)
        {
            var referer = Request.Headers.Referer;
            s_wasmClientURL = referer[0];

            // Build the URL to which the customer will be redirected after paying.
            var server = sp.GetRequiredService<IServer>();

            var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

            string? thisApiUrl = null;

            if (serverAddressesFeature is not null)
            {
                thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();
            }

            if (thisApiUrl is not null)
            {
                var userId = GetUserId();

                string sessionId = string.Empty;

                if (isSubscription.Equals(false) || isSubscription == null)
                {
                    sessionId = await CheckBookOut(id, userId, thisApiUrl);
                }
                else
                {
                    sessionId = await CheckSubscriptionOut(id, userId, thisApiUrl);
                }


                var pubKey = _configuration["Stripe:PubKey"];

                var checkoutOrderResponse = new CheckoutOrderResponse()
                {
                    SessionId = sessionId,
                    PubKey = pubKey
                };

                return Ok(checkoutOrderResponse);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [NonAction]
        public async Task<string> CheckBookOut(int bookId, int userId, string thisApiUrl)
        {
            var serviceId = await _checkoutService.CreateBookCheckoutSessionAsync(bookId, userId, thisApiUrl, s_wasmClientURL);

            return serviceId;
        }

        [NonAction]
        public async Task<string> CheckSubscriptionOut(int subId, int userId, string thisApiUrl)
        {
            var serviceId = await _checkoutService.CreateSubscriptionCheckoutSessionAsync(subId, userId, thisApiUrl, s_wasmClientURL);

            return serviceId;
        }

        [AllowAnonymous]
        [HttpGet("success")]
        // Automatic query parameter handling from ASP.NET.
        public async Task<ActionResult> CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            StripeList<LineItem> lineItems = sessionService.ListLineItems(sessionId);

            var productService = new ProductService();
            var product = productService.Get(lineItems.Data[0].Price.ProductId);

            var IsBookId = Int32.TryParse(product.Metadata["bookId"], out int bookId);
            var IsSubId = Int32.TryParse(product.Metadata["subscriptionId"], out int subId);
            var userId = Int32.Parse(product.Metadata["userId"]);

            if (IsBookId)
            {
                _logger.LogInformation($"User {userId} bought book {bookId}");

                await _checkoutService.SuccessfulBookCheckoutSessionAsync(bookId, userId);
            } 
            else if (IsSubId)
            {
                _logger.LogInformation($"User {userId} bought subscription {sessionId}");

                await _checkoutService.SuccessfulSubCheckoutSessionAsync(subId, userId);
            }

            return Redirect(s_wasmClientURL + "success");
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(userId);
        }
    }
}
