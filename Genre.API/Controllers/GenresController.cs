using Genre.API.Models.Requests;
using Genre.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;

namespace Genre.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;
        private readonly ILogger<GenresController> _logger;

        public GenresController(IGenresService genresService, ILogger<GenresController> logger)
        {
            _genresService = genresService;
            _logger = logger;
        }

        [Authorize(Policy = "Genres.Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateGenreAsync([FromBody] CreateGenresRequest genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdGenre = await _genresService.CreateGenreAsync(genre);

                return Ok(createdGenre);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating genre");

                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreByIdAsync(int id)
        {
            try
            {
                var genre = await _genresService.GetGenreByIdAsync(id);

                return Ok(genre);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting genre");

                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetGenresAsync()
        {
            try
            {
                var genres = await _genresService.GetGenresAsync();

                return Ok(genres);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting genres");

                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "Genres.Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateGenreAsync([FromBody] UpdateGenresRequest genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedGenre = await _genresService.UpdateGenreAsync(genre);

                return Ok(updatedGenre);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating genre");

                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Policy = "Genres.Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenreAsync(int id)
        {
            try
            {
                var deleted = await _genresService.DeleteGenreAsync(id);

                return Ok(deleted);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting genre");

                return StatusCode(500, "Internal server error");
            }
        }
    }
}