using Book.API.Models.Dtos;
using Book.API.Services.Interfaces;
using System.Net.Http.Headers;
using Book.API.Data.Entities;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Services
{
    public class UsersService : IUsersService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IHttpClientFactory httpClientFactory, ILogger<UsersService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }


        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var client = await CreateClient(null);

            var response = await client.GetAsync($"{WebApiLinks.UsersApi}/api/v1/users/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NotFoundException($"User with id {id} not found");
            }

            try
            {
                var user = await response.Content.ReadFromJsonAsync<UserDto>();

                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<HttpClient> CreateClient(string? token)
        {
            var client = _httpClientFactory.CreateClient("UsersService");

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}
