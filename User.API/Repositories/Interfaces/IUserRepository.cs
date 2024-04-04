using User.API.Data.Entities;

namespace User.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Users> CreateUserAsync(Users user);
        Task<Users> GetUserByIdAsync(int id);
        Task<IEnumerable<Users>> GetUsersAsync();
        Task<Users> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(int id);
    }
}
