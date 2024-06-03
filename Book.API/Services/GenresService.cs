using Book.API.Models.Responses;
using Book.API.Services.Interfaces;
using System.Net.Http.Headers;
using Book.API.Data.Entities;
using SendGrid.Helpers.Errors.Model;
using Book.API.Models.Responses.GenresResponses;
using Book.API.Repositories.Interfaces;
using Book.API.Models.Requests.GenresRequests;

namespace Book.API.Services
{
    public class GenresService : IGenresService
    {
        private readonly IGenresRepository _genresRepository;
        private readonly ILogger<GenresService> _logger;

        public GenresService(IGenresRepository genresRepository, ILogger<GenresService> logger)
        {
            _genresRepository = genresRepository;
            _logger = logger;
        }


        public async Task<Genres> CreateGenreAsync(CreateGenreRequest request)
        {
            try
            {
                var genre = new Genres
                {
                    Name = request.Name
                };
                
                var result = await _genresRepository.CreateGenreAsync(genre);

                return result;
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
                var result = await _genresRepository.GetGenreByIdAsync(id);

                return result;
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
                var result = await _genresRepository.GetGenresAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Genres> UpdateGenreAsync(Genres request)
        {
            try
            {
                var result = await _genresRepository.UpdateGenreAsync(request);

                return result;
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
                await _genresRepository.DeleteGenreAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
