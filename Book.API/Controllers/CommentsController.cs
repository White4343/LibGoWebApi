using Asp.Versioning;
using Book.API.Models.Requests.CommentsRequests;
using Book.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Book.API.Models.Dtos;
using IdentityModel;

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

            var user = GetUserData();

            var createdComment = await _commentsService.CreateCommentAsync(comment, user);
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

        [HttpGet("user")]
        public async Task<IActionResult> GetCommentsByUserIdAsync()
        {
            var user = GetUserData();

            var comments = await _commentsService.GetCommentsByBookIdAsync(user.Id);

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

            var user = GetUserData();

            var updatedComment = await _commentsService.UpdateCommentAsync(comment, user);

            return Ok(updatedComment);
        }

        [Authorize(Policy = "Comments.Client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentAsync(int id)
        {
            var user = GetUserData();

            var deleted = await _commentsService.DeleteCommentAsync(id, user.Id);

            return Ok(deleted);
        }

        private UserDataDto GetUserData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userNickname = User.FindFirstValue("nickname");

            var user = new UserDataDto
            {
                Id = int.Parse(userId),
                Role = userRole,
                Nickname = userNickname
            };

            return user;
        }
    }
}