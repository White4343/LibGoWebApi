using Asp.Versioning;
using Book.API.Data.Entities;
using Book.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Book.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BookGenresController : ControllerBase
    {
        private readonly IBookGenresService _bookGenresService;
        private readonly ILogger<BookGenresController> _logger;

        public BookGenresController(IBookGenresService bookGenresService, ILogger<BookGenresController> logger)
        {
            _bookGenresService = bookGenresService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBookGenreAsync([FromBody] BookGenres bookGenre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserIdStrict();

            var createdBookGenre = await _bookGenresService.CreateBookGenreAsync(bookGenre, userId);

            return Ok(createdBookGenre);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookGenreByIdAsync(int id)
        {
            var userId = GetUserId();

            var bookGenre = await _bookGenresService.GetBookGenreByIdAsync(id, userId);

            return Ok(bookGenre);
        }

        [AllowAnonymous]
        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetBookGenresByBookIdAsync(int bookId)
        {
            var userId = GetUserId();

            var bookGenres = await _bookGenresService.GetBookGenresByBookIdAsync(bookId, userId);

            return Ok(bookGenres);
        }

        [AllowAnonymous]
        [HttpGet("genre/{genreId}")]
        public async Task<IActionResult> GetBookGenresByGenreIdAsync(int genreId)
        {
            var userId = GetUserId();

            var bookGenres = await _bookGenresService.GetBookGenresByGenreIdAsync(genreId, userId);

            return Ok(bookGenres);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateBookGenreAsync([FromBody] BookGenres bookGenre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserIdStrict();

            var updatedBookGenre = await _bookGenresService.UpdateBookGenreAsync(bookGenre, userId);

            return Ok(updatedBookGenre);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookGenreAsync(int id)
        {
            var userId = GetUserIdStrict();

            await _bookGenresService.DeleteBookGenreAsync(id, userId);

            return Ok();
        }

        // TODO: Refactor GetUserId?
        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return -1;
            }

            return int.Parse(userId);
        }

        private int GetUserIdStrict()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(userId);
        }
    }
}
