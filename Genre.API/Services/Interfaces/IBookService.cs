using Genre.API.Data.Entities;

namespace Genre.API.Services.Interfaces
{
    public interface IBookService
    {
        Task<Books> GetBooksByGenresAsync(int genreId);
        Task<Books> GetBookByIdAsync(int id);
    }
}