using Asp.Versioning;
using Book.API.Models.Requests.ReadersRequests;
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
    public class ReadersController : ControllerBase
    {
        private readonly IReadersService _readersService;
        private readonly ILogger<ReadersController> _logger;

        public ReadersController(IReadersService readersService, ILogger<ReadersController> logger)
        {
            _readersService = readersService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReaderAsync([FromBody] CreateReadersRequest reader)
        {
            var userId = GetUserId();

            var createdReader = await _readersService.CreateReaderAsync(reader, userId);

            return Ok(createdReader);
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetReadersByUserIdAsync(int id)
        {
            var userId = GetUserId();

            var readers = await _readersService.GetReadersByUserIdAsync(id, userId);

            return Ok(readers);
        }

        [Authorize(Policy = "Readers.Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateReaderAsync([FromBody] UpdateReadersRequest reader)
        {
            var userId = GetUserId();

            var updatedReader = await _readersService.UpdateReaderAsync(reader, userId);

            return Ok(updatedReader);
        }

        [Authorize(Policy = "Readers.Client")]
        [HttpPatch("chapterId")]
        public async Task<IActionResult> PatchChapterAsync([FromBody] PatchReadersChapterId request)
        {
            var userId = GetUserId();

            var updatedReader = await _readersService.PatchReaderChapterAsync(request.Id, request.ChapterId, userId);

            return Ok(updatedReader);
        }

        [Authorize(Policy = "Readers.Client")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchReaderAsync([FromBody] PatchReadersRequest request)
        {
            var userId = GetUserId();

            var updatedReader = await _readersService.PatchReaderAsync(request, userId);

            return Ok(updatedReader);
        }

        [Authorize(Policy = "Readers.Client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReaderAsync(int id)
        {
            var userId = GetUserId();

            await _readersService.DeleteReaderAsync(id, userId);

            return Ok();
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null || userId.Length == 0)
            {
                return -1;
            }

            return int.Parse(userId);
        }
    }
}
