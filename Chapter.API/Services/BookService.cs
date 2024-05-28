using System.Net.Http.Headers;
using Chapter.API.Data.Entities;
using Chapter.API.Services.Interfaces;
using Genre.API;
using SendGrid.Helpers.Errors.Model;

namespace Chapter.API.Services
{
    public class BookService : IBookService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BookService> _logger;

        public BookService(IHttpClientFactory httpClientFactory, ILogger<BookService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Books> GetBookByIdAsync(int id, string? token)
        {
            var client = await CreateClient(token);

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
