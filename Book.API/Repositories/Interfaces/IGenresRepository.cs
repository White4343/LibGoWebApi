using Book.API.Data.Entities;

namespace Book.API.Repositories.Interfaces
{
    public interface IGenresRepository
    {
        Task<Genres> CreateGenreAsync(Genres genre);
        Task<Genres> GetGenreByIdAsync(int id);
        Task<IEnumerable<Genres>> GetGenresAsync();
        Task<Genres> UpdateGenreAsync(Genres genre);
        Task DeleteGenreAsync(int id);
    }
}
