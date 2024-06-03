using User.API.Models;

namespace User.API.Services.Interfaces
{
    public interface IBooksService
    {
        Task<Books> GetBookByIdAsync(int id);
    }
}