using System.Net.Http.Headers;
using Book.API.Models.Requests;
using Book.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Services
{
    public class BoughtBooksService : IBoughtBooksService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BoughtBooksService> _logger;

        public BoughtBooksService(IHttpClientFactory httpClientFactory, ILogger<BoughtBooksService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task GetBoughtBooksByBookId(int bookId, string? token)
        {
            if (token == null)
                throw new UnauthorizedAccessException("User authorization is required");

            string? tokenValue = token.Replace("Bearer ", "");

            var client = await CreateClient(tokenValue);

            var response = await client.GetAsync($"{WebApiLinks.UsersApi}/api/v1/boughtbooks/book/{bookId}");

            if (response.IsSuccessStatusCode)
            {
                throw new UnauthorizedException("You can't delete book while somebody bought it.");
            }
        }

        public async Task GetUserSubscriptionByBookId(int bookId, string? token)
        {
            string? tokenValue = token.Replace("Bearer ", "");

            var client = await CreateClient(tokenValue);

            var response = await client.GetAsync($"{WebApiLinks.UsersApi}/api/v1/usersubscriptions/book/{bookId}");

            if (response.IsSuccessStatusCode)
            {
                var subscription = await response.Content.ReadFromJsonAsync<IEnumerable<GetUserSubcsriptionClientRequest>>();

                var result = subscription.Where(ss => ss.IsActive);

                if (result.Any())
                {
                    throw new UnsupportedContentTypeException("You can't delete book while somebody bought it.");
                }
            }
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
