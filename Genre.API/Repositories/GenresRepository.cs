using AutoMapper;
using Genre.API.Data;
using Genre.API.Data.Entities;
using Genre.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace Genre.API.Repositories
{
    public class GenresRepository : IGenresRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GenresRepository> _logger;
        private readonly IMapper _mapper;

        public GenresRepository(AppDbContext context, ILogger<GenresRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<Genres> CreateOrderAsync(Genres genre)
        {
            var genreToCreate = await _context.Genres.AddAsync(genre);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Genre {genre.Name} with id {genre.Id} is created");

            return genreToCreate.Entity;
        }

        public async Task<Genres> GetGenreByIdAsync(int id)
        {
            try
            {
                var genre = await GenreExists(id);

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
            var genres = await _context.Genres.ToListAsync();

            if (genres == null)
            {
                throw new NotFoundException("No genres found.");
            }

            return genres;
        }

        public async Task<Genres> UpdateGenreAsync(Genres genre)
        {
            try
            {
                var genreToUpdate = await GenreExists(genre.Id);

                _context.Genres.Update(genre);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Genre {genre.Name} with id {genre.Id} is updated");

                return genreToUpdate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteGenreAsync(int id)
        {
            try
            {
                var genre = await GenreExists(id);

                _context.Genres.Remove(genre);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Genre {genre.Name} with id {genre.Id} is deleted");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Genres> GenreExists(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                throw new NotFoundException($"Genre with id: {id}, not found.");
            }
            
            return genre;
        }
    }
}