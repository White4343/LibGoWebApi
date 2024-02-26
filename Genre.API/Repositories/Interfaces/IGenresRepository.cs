using Genre.API.Data.Entities;

namespace Genre.API.Repositories.Interfaces
{
    public interface IGenresRepository
    {
        Task<Genres> CreateOrderAsync(Genres genre);
        Task<Genres> GetGenreByIdAsync(int id);
        Task<IEnumerable<Genres>> GetGenresAsync();
        Task<Genres> UpdateGenreAsync(Genres genre);
        Task<bool> DeleteGenreAsync(int id);
    }
}