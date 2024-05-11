using Asp.Versioning;
using Book.API.Models.Requests.BooksRequests;
using Book.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


// TODO: Add Caching
namespace Book.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBooksService booksService, ILogger<BooksController> logger)
        {
            _booksService = booksService;
            _logger = logger;
        }

        [Authorize(Policy = "Books.Client")]
        [HttpPost]
        public async Task<IActionResult> CreateBookAsync([FromBody] CreateBooksRequest book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = GetUserId();

            var createdBook = await _booksService.CreateBookAsync(book, userId);
            return Ok(createdBook);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookByIdAsync(int id)
        {
            var userId = GetUserId();

            var book = await _booksService.GetBookByIdAsync(id, userId);

            return Ok(book);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBooksAsync()
        {
            var books = await _booksService.GetBooksAsync();

            return Ok(books);
        }

        [AllowAnonymous]
        [HttpGet("page/{id}")]
        public async Task<IActionResult> GetBookPageByIdAsync(int id)
        {
            int userId = GetUserId();

            var book = await _booksService.GetBookPageByIdAsync(id, userId);

            return Ok(book);
        }

        [AllowAnonymous]
        [HttpGet("genre/{genreId}")]
        public async Task<IActionResult> GetBooksByGenreIdAsync(int genreId)
        {
            var books = await _booksService.GetGenreBooksPageByIdAsync(genreId);

            return Ok(books);
        }

        [AllowAnonymous]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBooksByUserIdAsync(int userId)
        {
            int tokenUserId = GetUserId();

            var books = await _booksService.GetBooksByUserIdAsync(userId, tokenUserId);

            return Ok(books);
        }

        [Authorize(Policy = "Books.Client")]
        [HttpPut]
        public async Task<IActionResult> UpdateBookAsync([FromBody] UpdateBooksRequest book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = GetUserId();

            var updatedBook = await _booksService.UpdateBookAsync(book, userId);
            return Ok(updatedBook);
        }

        [Authorize(Policy = "Books.Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookAsync(int id)
        {
            int userId = GetUserId();

            var deleted = await _booksService.DeleteBookAsync(id, userId);

            return Ok(deleted);
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return -1;
            }

            return int.Parse(userId);
        }
    }
}
