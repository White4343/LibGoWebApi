using Asp.Versioning;
using Chapter.API.Data.Entities;
using Chapter.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Chapter.API.Models.Requests;


// TODO: Add Caching
namespace Chapter.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ChaptersController : ControllerBase
    {
        private readonly IChapterService _chapterService;
        private readonly ILogger<ChaptersController> _logger;

        public ChaptersController(IChapterService chapterService, ILogger<ChaptersController> logger)
        {
            _chapterService = chapterService;
            _logger = logger;
        }

        [Authorize(Policy = "Chapters.Client")]
        [HttpPost]
        public async Task<IActionResult> CreateChapterAsync([FromBody] CreateChapterRequest chapter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserIdStrict();

            var token = GetToken();

            var createdChapter = await _chapterService.CreateChapterAsync(chapter, userId, token);

            return Ok(createdChapter);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapterByIdAsync(int id)
        {
            var userId = GetUserId();

            var token = GetToken();

            var chapter = await _chapterService.GetChapterByIdAsync(id, userId, token);

            return Ok(chapter);
        }

        [AllowAnonymous]
        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetChaptersByBookIdAsync(int bookId)
        {
            var userId = GetUserId();

            var token = GetToken();

            var chapters = await _chapterService.GetChaptersByBookIdAsync(bookId, userId, token);

            return Ok(chapters);
        }

        [AllowAnonymous]
        [HttpGet("book/titles/{bookId}")]
        public async Task<IActionResult> GetChapterTitlesByBookIdAsync(int bookId)
        {
            var chapters = await _chapterService.GetChaptersTitlesByBookIdAsync(bookId);

            return Ok(chapters);
        }

        [Authorize(Policy = "Chapters.Client")]
        [HttpPut]
        public async Task<IActionResult> UpdateChapterAsync([FromBody] UpdateChapterRequest chapter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetUserIdStrict();

            var token = GetToken();

            var updatedChapter = await _chapterService.UpdateChapterAsync(chapter, userId, token);

            return Ok(updatedChapter);
        }

        [Authorize(Policy = "Chapters.Client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapterAsync(int id)
        {
            var userId = GetUserIdStrict();

            var token = GetToken();

            var deleted = await _chapterService.DeleteChapterAsync(id, userId, token);

            return Ok(deleted);
        }

        [Authorize(Policy = "Chapters.Client")]
        [HttpDelete("book/{bookId}")]
        public async Task<IActionResult> DeleteChaptersByBookIdAsync(int bookId)
        {
            var userId = GetUserIdStrict();
            
            var token = GetToken();

            var deleted = await _chapterService.DeleteChaptersByBookIdAsync(bookId, userId, token);

            return Ok(deleted);
        }

        private string? GetToken()
        {
            return HttpContext.Request.Headers["Authorization"].ToString();
        }

        private int? GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return null;
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
