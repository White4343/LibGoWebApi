using Book.API.Data;
using Book.API.Data.Entities;
using Book.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Repositories
{
    public class GenresRepository : IGenresRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GenresRepository> _logger;

        public GenresRepository(AppDbContext context, ILogger<GenresRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Genres> CreateGenreAsync(Genres genre)
        {
            try
            {
                _context.Genres.Add(genre);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Genre with ID '{genre.Id}' and with name '{genre.Name}' was added");

                return genre;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Genres> GetGenreByIdAsync(int id)
        {
            try
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);

                if (genre == null)
                {
                    throw new NotFoundException($"Genre with id {id} not found");
                }

                return genre;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Genres>> GetGenresAsync()
        {
            try
            {
                var genres = await _context.Genres.ToListAsync();

                if (genres == null || genres.Count == 0)
                {
                    throw new NotFoundException("Genres not found");
                }

                return genres;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Genres> UpdateGenreAsync(Genres genre)
        {
            try
            {
                await GetGenreByIdAsync(genre.Id);

                _context.Genres.Update(genre);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Genre with ID '{genre.Id}' and with name '{genre.Name}' was updated");

                return genre;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteGenreAsync(int id)
        {
            try
            {
                var genre = await GetGenreByIdAsync(id);

                _context.Genres.Remove(genre);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Genre with ID '{genre.Id}' and with name '{genre.Name}' was deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}