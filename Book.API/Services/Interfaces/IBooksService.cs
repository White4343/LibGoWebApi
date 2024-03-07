using Book.API.Data.Entities;
using Book.API.Models.Requests;
using Book.API.Models.Responses;
using Book.API.Models.Responses.GenresResponses;

namespace Book.API.Services.Interfaces
{
    public interface IBooksService
    {
        Task<Books> CreateBookAsync(CreateBooksRequest book, int userId);
        Task<Books> GetBookByIdAsync(int id);
        Task<GetBookByPageResponse> GetBookPageByIdAsync(int bookId);
        Task<GetBooksByGenreResponse> GetGenreBooksPageByIdAsync(int genreId);
        Task<IEnumerable<Books>> GetBooksAsync();
        Task<Books> UpdateBookAsync(UpdateBooksRequest book, int userId);
        Task<bool> DeleteBookAsync(int id, int userId);
    }
}