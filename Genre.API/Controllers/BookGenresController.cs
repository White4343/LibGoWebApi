using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Genre.API.Controllers
{
    [Authorize(Policy = "BookGenres.Client")]
    [ApiController]
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

        [HttpPost]
        public async Task<IActionResult> CreateBookGenreAsync([FromBody] CreateBookGenresRequest bookGenre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserId();

            var createdBookGenre =
                await _bookGenresService.CreateBookGenreAsync(bookGenre, userId);

            return Ok(createdBookGenre);
            
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookGenreByIdAsync(int id)
        {

            var bookGenre = await _bookGenresService.GetBookGenreByIdAsync(id);

            return Ok(bookGenre);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBookGenresAsync()
        {
            var bookGenres = await _bookGenresService.GetBookGenresAsync();

            return Ok(bookGenres);
        }

        [AllowAnonymous]
        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetBookGenresByBookIdAsync(int bookId)
        {
            var bookGenres = await _bookGenresService.GetBookGenresByBookIdAsync(bookId);

            return Ok(bookGenres);
        }

        [AllowAnonymous]
        [HttpGet("genre/{genreId}")]
        public async Task<IActionResult> GetBookGenresByGenreIdAsync(int genreId)
        {
            var bookGenres = await _bookGenresService.GetBookGenresByGenreIdAsync(genreId);

            return Ok(bookGenres);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBookGenreAsync([FromBody] UpdateBookGenresRequest bookGenre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = GetUserId();

            var updatedBookGenre = await _bookGenresService.UpdateBookGenreAsync(bookGenre, userId);

            return Ok(updatedBookGenre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookGenreAsync(int id)
        {

            int userId = GetUserId();

            var deleted = await _bookGenresService.DeleteBookGenreAsync(id, userId);

            return Ok(deleted);
        }

        [HttpDelete("book/{bookId}")]
        public async Task<IActionResult> DeleteBookGenresByBookIdAsync(int bookId)
        {
            int userId = GetUserId();

            var deleted = await _bookGenresService.DeleteBookGenresByBookIdAsync(bookId, userId);

            return Ok(deleted);
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(userId);
        }
    }
}