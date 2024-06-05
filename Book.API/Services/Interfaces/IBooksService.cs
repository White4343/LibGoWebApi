using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests.BooksRequests;
using Book.API.Models.Responses.BooksResponses;
using Book.API.Models.Responses.GenresResponses;

namespace Book.API.Services.Interfaces
{
    public interface IBooksService
    {
        Task<Books> CreateBookAsync(CreateBooksRequest book, int userId);
        Task<Books> GetBookByIdAsync(int id, int userId);
        Task<IEnumerable<Books>> GetBooksByUserIdAsync(int id, int userId);
        Task<IEnumerable<Books>> GetBooksByGenreAsync(IEnumerable<BookGenresDto> bookGenres);
        Task<IEnumerable<Books>> GetBooksAsync();
        Task<IEnumerable<Books>> GetBooksByBookNameAsync(string name);
        Task<Books> UpdateBookAsync(UpdateBooksRequest book, int userId);
        Task<bool> DeleteBookAsync(int id, int userId, string token);
    }
}