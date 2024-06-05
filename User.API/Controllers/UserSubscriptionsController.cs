using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.API.Services.Interfaces;

namespace User.API.Controllers
{
    // TODO: Add expired when user access to userSubscriptions
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserSubscriptionsController : ControllerBase
    {
        private readonly IUserSubscriptionsService _userSubscriptionsService;
        private readonly ILogger<UserSubscriptionsController> _logger;

        public UserSubscriptionsController(IUserSubscriptionsService userSubscriptionsService,
            ILogger<UserSubscriptionsController> logger)
        {
            _userSubscriptionsService = userSubscriptionsService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserSubscriptionById(int id)
        {
            var userId = GetUserId();

            var userSubscription = await _userSubscriptionsService.GetUserSubscriptionByIdAsync(id, userId);

            return Ok(userSubscription);
        }

        [HttpGet("user/{userId}/book/{bookId}")]
        public async Task<IActionResult> GetUserSubscriptionByUserIdBookId(int userId, int bookId)
        {
            var tokenUserId = GetUserId();

            var userSubscription = await _userSubscriptionsService.GetUserSubscriptionByUserIdBookIdAsync(userId, bookId, tokenUserId);

            return Ok(userSubscription);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserSubscriptionsByUserId(int userId)
        {
            var tokenUserId = GetUserId();

            var userSubscriptions = await _userSubscriptionsService.GetUserSubscriptionsByUserIdAsync(userId, tokenUserId);

            return Ok(userSubscriptions);
        }

        [HttpGet("author/{authorUserId}")]
        public async Task<IActionResult> GetUserSubscriptionsByAuthorUserId(int authorUserId)
        {
            var tokenUserId = GetUserId();

            var userSubscriptions = await _userSubscriptionsService.GetUserSubscriptionsByAuthorUserIdAsync(authorUserId, tokenUserId);

            return Ok(userSubscriptions);
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetUserSubscriptionsByBookId(int bookId)
        {
            var tokenUserId = GetUserId();

            var userSubscriptions = await _userSubscriptionsService.GetUserSubscriptionsByBookIdAsync(bookId, tokenUserId);

            return Ok(userSubscriptions);
        }

        [Authorize(Policy = "Users.Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUserSubscriptions()
        {
            var userSubscriptions = await _userSubscriptionsService.GetUserSubscriptionsAsync();

            return Ok(userSubscriptions);
        }

        [Authorize(Policy = "Users.Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserSubscriptionById(int id)
        {
            await _userSubscriptionsService.DeleteUserSubscriptionByIdAsync(id);

            return Ok();
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(userId);
        }
    }
}
