using User.API.Data.Entities;
using User.API.Models.Dtos;
using User.API.Models.Requests;
using User.API.Models.Responses;

namespace User.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserRequest user);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<GetUserPrivatePageResponse> GetUserPrivatePageByIdAsync(int id, int tokenUserId);
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<Users> UpdateUserAsync(Users user, int tokenUserId);
        Task<Users> PatchUserAsync(PatchUserRequest request, int tokenUserId);
        Task<Users> PatchUserPasswordAsync(PatchUserPasswordRequest request, int tokenUserId);
        Task<bool> DeleteUserAsync(int id, int tokenUserId);
    }
}