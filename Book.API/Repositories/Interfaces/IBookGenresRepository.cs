using Book.API.Data.Entities;

namespace Book.API.Repositories.Interfaces
{
    public interface IBookGenresRepository
    {
        Task<BookGenres> CreateBookGenreAsync(BookGenres bookGenre);
        Task<BookGenres> GetBookGenreByIdAsync(int id);
        Task<IEnumerable<BookGenres>> GetBookGenresAsync();
        Task<IEnumerable<BookGenres>> GetBookGenresByBookIdAsync(int bookId);
        Task<IEnumerable<BookGenres>> GetBookGenresByGenreIdAsync(int genreId);
        Task<BookGenres> UpdateBookGenreAsync(BookGenres bookGenre);
        Task DeleteBookGenreAsync(int id);
    }
}
