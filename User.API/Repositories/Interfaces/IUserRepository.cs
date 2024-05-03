using User.API.Data.Entities;
using User.API.Models.Requests;

namespace User.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Users> CreateUserAsync(Users user);
        Task<Users> GetUserByIdAsync(int id);
        Task<IEnumerable<Users>> GetUsersAsync();
        Task<Users> UpdateUserAsync(Users user);
        Task<Users> PatchUserAsync(int id, string field, string value);
        Task<Users> PatchUserPasswordAsync(PatchUserPasswordRequest request);
        Task<bool> DeleteUserAsync(int id);
    }
}