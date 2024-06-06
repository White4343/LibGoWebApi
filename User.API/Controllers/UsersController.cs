using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.API.Data.Entities;
using User.API.Models.Requests;
using User.API.Services.Interfaces;

namespace User.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest user)
        {
            var createdUser = await _userService.CreateUserAsync(user);

            return Ok(createdUser);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            return Ok(user);
        }

        [HttpGet("private/{id}")]
        public async Task<IActionResult> GetUserPrivatePageById(int id)
        {
            var tokenUserId = GetUserId();

            var user = await _userService.GetUserPrivatePageByIdAsync(id, tokenUserId);

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("search/{nickname}")]
        public async Task<IActionResult> SearchUsers(string nickname)
        {
            var users = await _userService.GetUsersByNicknameAsync(nickname);

            return Ok(users);
        }

        [Authorize(Policy = "Users.Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] Users user)
        {
            var tokenUserId = GetUserId();

            var updatedUser = await _userService.UpdateUserAsync(user, tokenUserId);

            return Ok(updatedUser);
        }

        [HttpPatch]
        public async Task<IActionResult> PatchUser([FromBody] PatchUserRequest userPatch)
        {
            var tokenUserId = GetUserId();

            var updatedUser = await _userService.PatchUserAsync(userPatch, tokenUserId);

            return Ok(updatedUser);
        }

        [HttpPatch("password")]
        public async Task<IActionResult> PatchUserPassword([FromBody] PatchUserPasswordRequest userPatch)
        {
            var tokenUserId = GetUserId();

            var updatedUser = await _userService.PatchUserPasswordAsync(userPatch, tokenUserId);

            return Ok(updatedUser);
        }

        [Authorize(Policy = "Users.Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var tokenUserId = GetUserId();

            var isDeleted = await _userService.DeleteUserAsync(id, tokenUserId);

            return Ok(isDeleted);
        }

        private int GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(userId);
        }
    }
}
