using AutoMapper;
using Genre.API.Data.Entities;
using Genre.API.Models.Requests;
using Genre.API.Repositories.Interfaces;
using Genre.API.Services.Interfaces;

namespace Genre.API.Services
{
    public class GenresService : IGenresService
    {
        private readonly IGenresRepository _genresRepository;
        private readonly ILogger<GenresService> _logger;
        private readonly IMapper _mapper;

        public GenresService(IGenresRepository genresRepository, ILogger<GenresService> logger, IMapper mapper)
        {
            _genresRepository = genresRepository;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<Genres> CreateGenreAsync(CreateGenresRequest genre)
        {
            var genreToCreate = _mapper.Map<Genres>(genre);

            var createdGenre = await _genresRepository.CreateOrderAsync(genreToCreate);

            return createdGenre;
        }

        public async Task<Genres> GetGenreByIdAsync(int id)
        {
            try
            {
                var genre = await _genresRepository.GetGenreByIdAsync(id);

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
                var genres = await _genresRepository.GetGenresAsync();

                return genres;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Genres> UpdateGenreAsync(UpdateGenresRequest genre)
        {
            try
            {
                var genreToUpdate = _mapper.Map<Genres>(genre);

                var updatedGenre = await _genresRepository.UpdateGenreAsync(genreToUpdate);

                return updatedGenre;
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
                var deleted = await _genresRepository.DeleteGenreAsync(id);

                return deleted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}