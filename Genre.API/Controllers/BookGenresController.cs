using Genre.API.Models.Requests;
using Genre.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Security.Claims;
using Genre.API.Data.Entities;

namespace Genre.API.Controllers
{
    [Authorize(Policy = "BookGenres.Client")]
    [ApiController]
    [Route("api/v1/[controller]")]
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

            try
            {
                var userId = GetUserId();

                var createdBookGenre =
                    await _bookGenresService.CreateBookGenreAsync(bookGenre, userId);

                return Ok(createdBookGenre);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid user id");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating book genre");

                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookGenreByIdAsync(int id)
        {
            try
            {
                var bookGenre = await _bookGenresService.GetBookGenreByIdAsync(id);

                return Ok(bookGenre);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting book genre");

                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBookGenresAsync()
        {
            try
            {
                var bookGenres = await _bookGenresService.GetBookGenresAsync();

                return Ok(bookGenres);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting book genres");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBookGenreAsync([FromBody] UpdateBookGenresRequest bookGenre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int userId = GetUserId();

                var updatedBookGenre = await _bookGenresService.UpdateBookGenreAsync(bookGenre, userId);

                return Ok(updatedBookGenre);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating book genre");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookGenreAsync(int id)
        {
            try
            {
                int userId = GetUserId();

                var deleted = await _bookGenresService.DeleteBookGenreAsync(id, userId);

                return Ok(deleted);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting book genre");

                return StatusCode(500, "Internal server error");
            }
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(userId);
        }
    }
