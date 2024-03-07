using Book.API.Models.Responses;
using Book.API.Models.Responses.GenresResponses;

namespace Book.API.Services.Interfaces
{
    public interface IGenresService
    {
        Task<GetGenresByBookIdResponse> GetGenresByBookIdAsync(int bookId, string token);
        Task<GetBooksByGenreIdResponse> GetBooksByGenreIdAsync(int genreId, string token);
    }
}