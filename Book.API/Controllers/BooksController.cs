using Asp.Versioning;
using Book.API.Models.Requests;
using Book.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            var createdBook = await _booksService.CreateBookAsync(book, 1);
            return Ok(createdBook);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookByIdAsync(int id)
        {
            var book = await _booksService.GetBookByIdAsync(id);

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
            var book = await _booksService.GetBookPageByIdAsync(id);

            return Ok(book);
        }

        [AllowAnonymous]
        [HttpGet("genre/{genreId}")]
        public async Task<IActionResult> GetBooksByGenreIdAsync(int genreId)
        {
            var books = await _booksService.GetGenreBooksPageByIdAsync(genreId);

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

            var updatedBook = await _booksService.UpdateBookAsync(book, 1);
            return Ok(updatedBook);
        }

        [Authorize(Policy = "Books.Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookAsync(int id)
        {
            var deleted = await _booksService.DeleteBookAsync(id, 1);

            return Ok(deleted);
        }
    }
}
