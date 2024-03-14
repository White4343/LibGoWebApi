using Chapter.API.Data.Entities;
using Chapter.API.Models.Requests;

namespace Chapter.API.Services.Interfaces
{
    public interface IChapterService
    {
        Task<Chapters> CreateChapterAsync(CreateChapterRequest chapter, int userId);
        Task<Chapters> GetChapterByIdAsync(int id);
        Task<IEnumerable<Chapters>> GetChaptersByBookIdAsync(int bookId);
        Task<Chapters> UpdateChapterAsync(UpdateChapterRequest chapter, int userId);
        Task<bool> DeleteChapterAsync(int id, int userId);
        Task<bool> DeleteChaptersByBookIdAsync(int bookId, int userId);
    }
}