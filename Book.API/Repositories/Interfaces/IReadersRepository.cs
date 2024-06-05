using Book.API.Data.Entities;
using Book.API.Models.Requests.ReadersRequests;

namespace Book.API.Repositories.Interfaces
{
    public interface IReadersRepository
    {
        Task<Readers> CreateReaderAsync(Readers reader);
        Task<Readers> GetReaderByIdAsync(int id);
        Task<Readers> GetReaderByUserIdAndBookIdAsync(int userId, int bookId);
        Task<IEnumerable<Readers>> GetReadersAsync();
        Task<IEnumerable<Readers>> GetReadersByUserIdAsync(int id);
        Task<IEnumerable<Readers>> GetReadersByBookIdAsync(int id);
        Task<double> GetBooksRatingByBookIdAsync(int bookId);
        Task<Readers> UpdateReaderAsync(Readers reader);
        Task<Readers> PatchReaderAsync(PatchReadersRequest reader);
        Task<Readers> PatchReaderChapterIdAsync(int id, int chapterId);
        Task<bool> DeleteReaderAsync(int id);
    }
}