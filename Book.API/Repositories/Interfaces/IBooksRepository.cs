using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests;

namespace Book.API.Repositories.Interfaces
{
    public interface IBooksRepository
    {
        Task<Books> CreateBookAsync(Books book);
        Task<Books> GetBookByIdAsync(int id);
        Task<IEnumerable<Books>> GetBooksAsync();
        Task<IEnumerable<Books>> GetBooksByGenreAsync(IEnumerable<BookGenresDto> bookGenres);
        Task<IEnumerable<Books>> GetBooksByUserIdAsync(int id);
        Task<Books> UpdateBookAsync(Books book);
        Task<bool> DeleteBookAsync(int id);
    }
}