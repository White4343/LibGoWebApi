using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.API.Services.Interfaces;

namespace User.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BoughtBooksController : ControllerBase
    {
        private readonly IBoughtBooksService _boughtBooksService;

        public BoughtBooksController(IBoughtBooksService boughtBooksService)
        {
            _boughtBooksService = boughtBooksService;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetBoughtBooksByUserIdAsync(int id)
        {
            var userId = GetUserId();

            var books = await _boughtBooksService.GetBoughtBooksByUserId(id, userId);

            return Ok(books);
        }

        [HttpGet("book/{id}")]
        public async Task<IActionResult> GetBoughtBooksByBookIdAsync(int id)
        {
            var userId = GetUserId();

            var books = await _boughtBooksService.GetBoughtBooksByBookId(id, userId);

            return Ok(books);
        }

        [HttpGet("user/{userId}/book/{bookId}")]
        public async Task<IActionResult> GetBoughtBooksByUserIdByBookIdAsync(int userId, int bookId)
        {
            var tokenUserId = GetUserId();

            var book = await _boughtBooksService.GetBoughtBooksByUserIdByBookId(userId, bookId, tokenUserId);

            return Ok(book);
        }

        [Authorize(Policy = "Users.Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoughtBooksByIdAsync(int id)
        {
            var book = await _boughtBooksService.GetBoughtBooksById(id);

            return Ok(book);
        }

        [Authorize(Policy = "Users.Admin")]
        [HttpGet]
        public async Task<IActionResult> GetBoughtBooksAsync()
        {
            var books = await _boughtBooksService.GetBoughtBooks();

            return Ok(books);
        }

        [Authorize(Policy = "Users.Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoughtBooksByIdAsync(int id)
        {
            await _boughtBooksService.DeleteBoughtBooksById(id);

            return Ok();
        }

        [Authorize(Policy = "Users.Admin")]
        [HttpDelete("user/{userId}/book/{bookId}")]
        public async Task<IActionResult> DeleteBoughtBooksByUserIdByBookIdAsync(int userId, int bookId)
        {
            await _boughtBooksService.DeleteBoughtBooksByUserIdByBookId(userId, bookId);

            return Ok();
        }

        [Authorize(Policy = "Users.Admin")]
        [HttpDelete("subscription/{subscriptionId}")]
        public async Task<IActionResult> DeleteBoughtBooksBySubscriptionIdAsync(int subscriptionId)
        {
            await _boughtBooksService.DeleteBoughtBooksBySubscriptionId(subscriptionId);

            return Ok();
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(userId);
        }
    }
}
