using Genre.API.Data.Entities;
using Genre.API.Models.Requests;

namespace Genre.API.Services.Interfaces
{
    public interface IGenresService
    {
        Task<Genres> CreateGenreAsync(CreateGenresRequest genre);
        Task<Genres> GetGenreByIdAsync(int id);
        Task<IEnumerable<Genres>> GetGenresAsync();
        Task<Genres> UpdateGenreAsync(UpdateGenresRequest genre);
        Task<bool> DeleteGenreAsync(int id);
    }
}