using Asp.Versioning;
using Book.API.Models.Requests.CommentsRequests;
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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ICommentsService commentsService, ILogger<CommentsController> logger)
        {
            _commentsService = commentsService;
            _logger = logger;
        }

        [Authorize(Policy = "Comments.Client")]
        [HttpPost]
        public async Task<IActionResult> CreateCommentAsync([FromBody] CreateCommentsRequest comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = GetUserId();

            var createdComment = await _commentsService.CreateCommentAsync(comment, userId);
            return Ok(createdComment);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentByIdAsync(int id)
        {
            var comment = await _commentsService.GetCommentByIdAsync(id);

            return Ok(comment);
        }

        [AllowAnonymous]
        [HttpGet("book/{id}")]
        public async Task<IActionResult> GetCommentsByBookIdAsync(int id)
        {
            var comments = await _commentsService.GetCommentsByBookIdAsync(id);

            return Ok(comments);
        }

        [AllowAnonymous]
        [HttpGet("user")]
        public async Task<IActionResult> GetCommentsByUserIdAsync()
        {
            int userId = GetUserId();

            var comments = await _commentsService.GetCommentsByBookIdAsync(userId);

            return Ok(comments);
        }

        [Authorize(Policy = "Comments.Client")]
        [HttpPut]
        public async Task<IActionResult> UpdateCommentAsync([FromBody] UpdateCommentsRequest comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int userId = GetUserId();

            var updatedComment = await _commentsService.UpdateCommentAsync(comment, userId);

            return Ok(updatedComment);
        }

        [Authorize(Policy = "Comments.Client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentAsync(int id)
        {
            int userId = GetUserId();

            var deleted = await _commentsService.DeleteCommentAsync(id, userId);

            return Ok(deleted);
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(userId);
        }
    }
}
