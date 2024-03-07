using Book.API.Models.Responses;
using Book.API.Services.Interfaces;
using System.Net.Http.Headers;
using SendGrid.Helpers.Errors.Model;
using Book.API.Models.Responses.GenresResponses;

namespace Book.API.Services
{
    public class GenresService : IGenresService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<GenresService> _logger;

        public GenresService(IHttpClientFactory clientFactory, ILogger<GenresService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }


        public async Task<GetGenresByBookIdResponse> GetGenresByBookIdAsync(int bookId, string token)
        {
            var client = await CreateClient(token);
            var response = await client.GetAsync($"{WebApiLinks.GenresApi}/api/v1/bookgenres/book/{bookId}");

            if (response.IsSuccessStatusCode)
            {
                var genres = await response.Content.ReadFromJsonAsync<GetGenresByBookIdResponse>();

                return genres;
            }

            throw new NotFoundException($"Genres not found for book with id {bookId}");
        }

        public async Task<GetBooksByGenreIdResponse> GetBooksByGenreIdAsync(int genreId, string token)
        {
            var client = await CreateClient(token);
            var response = await client.GetAsync($"{WebApiLinks.GenresApi}/api/v1/bookgenres/genre/{genreId}");

            if (response.IsSuccessStatusCode)
            {
                var books = await response.Content.ReadFromJsonAsync<GetBooksByGenreIdResponse>();

                return books;
            }

            throw new NotFoundException($"Books not found for genre with id {genreId}");
        }

        private async Task<HttpClient> CreateClient(string? token)
        {
            var client = _clientFactory.CreateClient("GenresService");

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}
