using Chapter.API.Data.Entities;
using Chapter.API.Models.Requests;
using Chapter.API.Models.Responses;

namespace Chapter.API.Services.Interfaces
{
    public interface IChapterService
    {
        Task<Chapters> CreateChapterAsync(CreateChapterRequest chapter, int userId, string? token);
        Task<Chapters> GetChapterByIdAsync(int id, int? userId, string? token);
        Task<IEnumerable<Chapters>> GetChaptersByBookIdAsync(int bookId, int? userId, string? token);
        Task<IEnumerable<GetChaptersTitlesByBookIdResponse>> GetChaptersTitlesByBookIdAsync(int bookId);
        Task<Chapters> UpdateChapterAsync(UpdateChapterRequest chapter, int userId, string? token);
        Task<bool> DeleteChapterAsync(int id, int userId, string? token);
        Task<bool> DeleteChaptersByBookIdAsync(int bookId, int userId, string? token);
    }
}