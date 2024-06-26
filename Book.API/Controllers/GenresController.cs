﻿using Asp.Versioning;
using Book.API.Data.Entities;
using Book.API.Models.Requests.GenresRequests;
using Book.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class GenresController : ControllerBase
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
        public async Task<IActionResult> CreateGenreAsync([FromBody] CreateGenreRequest genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdGenre = await _genresService.CreateGenreAsync(genre);

            return Ok(createdGenre);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreByIdAsync(int id)
        {
            var genre = await _genresService.GetGenreByIdAsync(id);

            return Ok(genre);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetGenresAsync()
        {
            var genres = await _genresService.GetGenresAsync();

            return Ok(genres);
        }

        [Authorize(Policy = "Genres.Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateGenreAsync([FromBody] Genres genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedGenre = await _genresService.UpdateGenreAsync(genre);

            return Ok(updatedGenre);
        }

        [Authorize(Policy = "Genres.Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenreAsync(int id)
        {
            await _genresService.DeleteGenreAsync(id);

            return Ok();
        }
    }
}
