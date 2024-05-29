using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Responses.BooksResponses;

namespace Book.API.Services.Interfaces
{
    public interface IBookGenresService
    {
        Task<BookGenresDto> CreateBookGenreAsync(BookGenres bookGenre, int tokenUserId);
        Task<BookGenresDto> GetBookGenreByIdAsync(int id, int tokenUserId);
        Task<IEnumerable<BookGenresDto>> GetBookGenresByGenreIdAsync(int genreId, int tokenUserId);
        Task<IEnumerable<BookGenresDto>> GetBookGenresByBookIdAsync(int bookId, int tokenUserId);
        Task<GetBookByPageResponse> GetBookPageByIdAsync(int bookId, int userId);
        Task<GetBooksByGenreResponse> GetGenreBooksPageByIdAsync(int genreId, int tokenUserId);
        Task<IEnumerable<GetAllBooksWithGenreNamesResponse>> GetAllBooksWithGenreNamesAsync();
        Task<BookGenresDto> UpdateBookGenreAsync(BookGenres bookGenre, int tokenUserId);
        Task DeleteBookGenreAsync(int id, int tokenUserId);
    }
}