using Chapter.API.Data.Entities;

namespace Chapter.API.Repositories.Interfaces
{
    public interface IChapterRepository
    {
        Task<Chapters> CreateChapterAsync(Chapters chapter);
        Task<Chapters> GetChapterByIdAsync(int id);
        Task<IEnumerable<Chapters>> GetChaptersByBookIdAsync(int bookId);
        Task<Chapters> UpdateChapterAsync(Chapters chapter);
        Task<bool> DeleteChapterAsync(int id);
        Task<bool> DeleteChaptersByBookIdAsync(int bookId);
    }
}