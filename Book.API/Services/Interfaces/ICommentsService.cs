using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests.CommentsRequests;

namespace Book.API.Services.Interfaces
{
    public interface ICommentsService
    {
        Task<CommentsDto> CreateCommentAsync(CreateCommentsRequest comment, int userId);
        Task<CommentsDto> GetCommentByIdAsync(int id);
        Task<IEnumerable<CommentsDto>> GetCommentsByBookIdAsync(int bookId);
        Task<IEnumerable<CommentsDto>> GetCommentsByUserIdAsync(int userId);
        Task<CommentsDto> UpdateCommentAsync(UpdateCommentsRequest comment, int userId);
        Task<bool> DeleteCommentAsync(int id, int userId);
        Task<bool> DeleteCommentsByBookIdAsync(int bookId, int userId);
        Task<bool> DeleteCommentsByUserIdAsync(int userId);
    }
}