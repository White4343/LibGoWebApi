using System.Net.Http.Headers;
using SendGrid.Helpers.Errors.Model;
using User.API.Models;
using User.API.Services.Interfaces;

namespace User.API.Services
{
    public class BooksService : IBooksService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BooksService> _logger;

        public BooksService(IHttpClientFactory httpClientFactory, ILogger<BooksService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Books> GetBookByIdAsync(int id)
        {
            var client = await CreateClient(null);

            var response = await client.GetAsync($"{WebApiLinks.BookApi}/api/v1/books/{id}");

            if (response.IsSuccessStatusCode)
            {
                var book = await response.Content.ReadFromJsonAsync<Books>();

                return book;
            }

            throw new NotFoundException($"Book with id {id} not found");

        }

        private async Task<HttpClient> CreateClient(string? token)
        {
            var client = _httpClientFactory.CreateClient("BooksService");

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}
