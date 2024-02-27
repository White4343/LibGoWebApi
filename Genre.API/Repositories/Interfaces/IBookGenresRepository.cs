namespace Genre.API.Repositories.Interfaces
{
    public interface IBookGenresRepository
    {
        Task<BookGenres> CreateBookGenreAsync(BookGenres bookGenre);
        Task<BookGenres> GetBookGenreByIdAsync(int id);
        Task<IEnumerable<BookGenres>> GetBookGenresAsync();
        Task<BookGenres> UpdateBookGenreAsync(BookGenres bookGenre);
        Task<bool> DeleteBookGenreAsync(int id);
    }
}