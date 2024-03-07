using System.Net.Http.Headers;
using SendGrid.Helpers.Errors.Model;

namespace Genre.API.Services
{
    public class BookService : IBookService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<BookService> _logger;

        public BookService(IHttpClientFactory clientFactory, ILogger<BookService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }


        public async Task<Books> GetBooksByGenresAsync(int genreId)
        {
            throw new NotImplementedException();
        }

        public async Task<Books> GetBookByIdAsync(int id)
        {
            try
            {
                var book = await BookExists(id, null);

                return book;
            }
            catch (NotFoundException e)
            {
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Books> BookExists(int id, string token)
        {
            var client = await CreateClient(token);
            var response = await client.GetAsync($"{WebApiLinks.BookApi}/api/v1/books/{id}");

            if (response.IsSuccessStatusCode)
            {
                var book = await response.Content.ReadFromJsonAsync<Books>();

                return book;
            }

            throw new NotFoundException($"Book not found with id {id}");
        }

        private async Task<HttpClient> CreateClient(string? token)
        {
            var client = _clientFactory.CreateClient("BooksService");

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}