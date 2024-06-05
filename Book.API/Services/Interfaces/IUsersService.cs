using Book.API.Models.Dtos;

namespace Book.API.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserDto> GetUserByIdAsync(int id);
    }
}
