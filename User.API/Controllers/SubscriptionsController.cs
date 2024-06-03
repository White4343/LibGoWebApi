using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.API.Models.Requests;
using User.API.Services.Interfaces;

namespace User.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionsService _subscriptionsService;
        private readonly ILogger<SubscriptionsController> _logger;

        public SubscriptionsController(ISubscriptionsService subscriptionsService,
            ILogger<SubscriptionsController> logger)
        {
            _subscriptionsService = subscriptionsService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequest request)
        {
            var tokenUserId = GetUserId();

            var createdSubscription = await _subscriptionsService.CreateSubscriptionAsync(request, tokenUserId);

            return Ok(createdSubscription);
        }

        [Authorize(Policy = "Users.Admin")]
        [HttpGet]
        public async Task<IActionResult> GetSubscriptions()
        {
            var subscriptions = await _subscriptionsService.GetSubscriptionsAsync();

            return Ok(subscriptions);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscriptionById(int id)
        {
            var subscription = await _subscriptionsService.GetSubscriptionByIdAsync(id);

            return Ok(subscription);
        }

        [AllowAnonymous]
        [HttpGet("/user/{userId}")]
        public async Task<IActionResult> GetSubscriptionByUserId(int userId)
        {
            var subscription = await _subscriptionsService.GetSubscriptionByUserIdAsync(userId);

            return Ok(subscription);
        }

        [AllowAnonymous]
        [HttpGet("/book/{bookId}")]
        public async Task<IActionResult> GetSubscriptionByBookId(int bookId)
        {
            var subscription = await _subscriptionsService.GetSubscriptionByBookIdAsync(bookId);

            return Ok(subscription);
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateSubscription([FromBody] UpdateSubscriptionRequest request)
        {
            var tokenUserId = GetUserId();

            var updatedSubscription = await _subscriptionsService.UpdateSubscriptionAsync(request, tokenUserId);

            return Ok(updatedSubscription);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var tokenUserId = GetUserId();

            var isDeleted = await _subscriptionsService.DeleteSubscriptionAsync(id, tokenUserId);

            return Ok(isDeleted);
        }

        [HttpDelete("/user/{userId}")]
        public async Task<IActionResult> DeleteSubscriptionByUserId(int userId)
        {
            var tokenUserId = GetUserId();

            var isDeleted = await _subscriptionsService.DeleteSubscriptionByUserIdAsync(userId, tokenUserId);

            return Ok(isDeleted);
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(userId);
        }
    }
}
