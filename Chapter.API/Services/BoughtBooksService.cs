using System.Net.Http.Headers;
using Chapter.API.Models.Requests;
using Chapter.API.Services.Interfaces;
using Genre.API;
using SendGrid.Helpers.Errors.Model;

namespace Chapter.API.Services
{
    public class BoughtBooksService : IBoughtBooksService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BookService> _logger;

        public BoughtBooksService(IHttpClientFactory httpClientFactory, ILogger<BookService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<GetBoughtBooksClientRequest> GetBoughtBooksByUserIdByBookId(int bookId, int? userId, string? token)
        {
            if (userId == null)
                throw new UnauthorizedAccessException("User authorization is required");

            string? tokenValue = token.Replace("Bearer ", "");

            var client = await CreateClient(tokenValue);

            var response = await client.GetAsync($"{WebApiLinks.UserApi}/api/v1/boughtbooks/user/{userId}/book/{bookId}");

            if (response.IsSuccessStatusCode)
            {
                var boughtBooks = await response.Content.ReadFromJsonAsync<GetBoughtBooksClientRequest>();

                return boughtBooks;
            }

            throw new NotFoundException($"Bought books with book id {bookId} and user id {userId} not found");
        }

        public async Task<GetUserSubcsriptionClientRequest> GetUserSubscriptionByUserId(int userId, int bookId, string? token)
        {
            string? tokenValue = token.Replace("Bearer ", "");

            var client = await CreateClient(tokenValue);

            var response = await client.GetAsync($"{WebApiLinks.UserApi}/api/v1/usersubscriptions/user/{userId}/book/{bookId}");

            if (response.IsSuccessStatusCode)
            {
                var subscription = await response.Content.ReadFromJsonAsync<GetUserSubcsriptionClientRequest>();

                if (!subscription.IsActive)
                {
                    throw new BadRequestException("Subscription is not active");
                }

                return subscription;
            }

            throw new NotFoundException($"Subscription with user id {userId} not found");
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
