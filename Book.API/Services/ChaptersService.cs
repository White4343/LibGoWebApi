using Book.API.Services.Interfaces;
using System.Net.Http.Headers;
using Book.API.Data.Entities;
using SendGrid.Helpers.Errors.Model;

// TODO: When chapter is not available, because book is not bough, we should return NotFoundException
namespace Book.API.Services
{
    public class ChaptersService : IChapterService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ChaptersService> _logger;

        public ChaptersService(IHttpClientFactory httpClientFactory, ILogger<ChaptersService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Chapters> GetChapterByIdAsync(int id)
        {
            var client = await CreateClient(null);

            var response = await client.GetAsync($"{WebApiLinks.ChaptersApi}/api/v1/chapters/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NotFoundException($"Chapter with id {id} not found");
            }

            try
            {
                var chapter = await response.Content.ReadFromJsonAsync<Chapters>();

                return chapter;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<HttpClient> CreateClient(string? token)
        {
            var client = _httpClientFactory.CreateClient("ChaptersService");

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}
