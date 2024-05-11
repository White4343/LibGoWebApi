using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests.ReadersRequests;
using Book.API.Models.Responses.ReadersResponses;

namespace Book.API.Services.Interfaces
{
    public interface IReadersService
    {
        Task<ReadersDto> CreateReaderAsync(CreateReadersRequest reader, int tokenUserId);
        Task<GetReadersBookByUserId> GetReadersByUserIdAsync(int id, int tokenUserId);
        Task<Readers> UpdateReaderAsync(UpdateReadersRequest reader, int userId);
        Task<Readers> PatchReaderAsync(PatchReadersRequest reader, int userId);
        Task<Readers> PatchReaderChapterAsync(int readerId, int chapterId, int userId);
        Task<bool> DeleteReaderAsync(int id, int userId);
    }
}