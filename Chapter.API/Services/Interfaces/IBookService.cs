using Chapter.API.Data.Entities;

namespace Chapter.API.Services.Interfaces
{
    public interface IBookService
    {
        Task<Books> GetBookByIdAsync(int id);
    }
}