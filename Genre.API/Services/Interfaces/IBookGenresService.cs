namespace Genre.API.Services.Interfaces
{
    public interface IBookGenresService
    {
        Task<BookGenres> CreateBookGenreAsync(CreateBookGenresRequest bookGenre, int userId);
        Task<BookGenres> GetBookGenreByIdAsync(int id);
        Task<IEnumerable<BookGenres>> GetBookGenresAsync();
        Task<BookGenres> UpdateBookGenreAsync(UpdateBookGenresRequest bookGenre, int userId);
        Task<bool> DeleteBookGenreAsync(int id, int userId);
    }
}