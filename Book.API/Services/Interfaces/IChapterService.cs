using Book.API.Data.Entities;

namespace Book.API.Services.Interfaces
{
    public interface IChapterService
    {
        Task<Chapters> GetChapterByIdAsync(int id);
    }
}