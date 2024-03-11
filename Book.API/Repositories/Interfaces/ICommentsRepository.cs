using Book.API.Data.Entities;

namespace Book.API.Repositories.Interfaces
{
    public interface ICommentsRepository
    {
        Task<Comments> CreateCommentAsync(Comments comment);
        Task<Comments> GetCommentByIdAsync(int id);
        Task<IEnumerable<Comments>> GetCommentsByBookIdAsync(int bookId);
        Task<IEnumerable<Comments>> GetCommentsByUserIdAsync(int userId);
        Task<Comments> UpdateCommentAsync(Comments comment);
        Task<bool> DeleteCommentAsync(int id, int userId);
        Task<bool> DeleteCommentsByBookIdAsync(int bookId, int userId);
        Task<bool> DeleteCommentsByUserIdAsync(int userId);
    }
}