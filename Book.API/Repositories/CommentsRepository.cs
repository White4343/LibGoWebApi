using Book.API.Data;
using Book.API.Data.Entities;
using Book.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CommentsRepository> _logger;

        public CommentsRepository(AppDbContext context, ILogger<CommentsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<Comments> CreateCommentAsync(Comments comment)
        {
            try
            {
                var commentToCreate = await _context.Comments.AddAsync(comment);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Comment with id {comment.Id} is created");

                return commentToCreate.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError($"Error creating comment with id {comment.Id}");
                throw;
            }
        }

        public async Task<Comments> GetCommentByIdAsync(int id)
        {
            try
            {
                var comment = await CommentExists(id);

                return comment;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Comments>> GetCommentsByBookIdAsync(int bookId)
        {
            try
            {
                var comments = await _context.Comments.Where(c => c.BookId == bookId).ToListAsync();

                if (comments == null || comments.Count == 0)
                {
                    throw new NotFoundException($"Comments for book with id {bookId} not found.");
                }

                return comments;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Comments>> GetCommentsByUserIdAsync(int userId)
        {
            try
            {
                var comments = await _context.Comments.Where(c => c.UserId == userId).ToListAsync();

                if (comments == null || comments.Count == 0)
                {
                    throw new NotFoundException($"Comments for user with id {userId} not found.");
                }

                return comments;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Comments> UpdateCommentAsync(Comments comment)
        {
            try
            {
                var commentToUpdate = await CommentExists(comment.Id);

                await IsCommentAuthor(commentToUpdate.UserId, comment.UserId);

                commentToUpdate.Content = comment.Content;

                commentToUpdate.UpdateDate = DateTime.UtcNow;

                _context.Comments.Update(commentToUpdate);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Comment with id {comment.Id} is updated");

                return commentToUpdate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError($"Error updating comment with id {comment.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteCommentAsync(int id, int userId)
        {
            try
            {
                var comment = await CommentExists(id);

                await IsCommentAuthor(comment.UserId, userId);

                _context.Comments.Remove(comment);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Comment with id {id} is deleted");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError($"Error deleting comment with id {id}");
                throw;
            }
        }

        public async Task<bool> DeleteCommentsByBookIdAsync(int bookId, int userId)
        {
            try
            {
                var comments = await GetCommentsByBookIdAsync(bookId);

                await IsCommentAuthor(comments.First().UserId, userId);

                _context.Comments.RemoveRange(comments);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Comments for book with id {bookId} are deleted");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError($"Error deleting comments for book with id {bookId}");
                throw;
            }
        }

        public async Task<bool> DeleteCommentsByUserIdAsync(int userId)
        {
            try
            {
                var comments = await GetCommentsByUserIdAsync(userId);

                _context.Comments.RemoveRange(comments);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Comments for user with id {userId} are deleted");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError($"Error deleting comments for user with id {userId}");
                throw;
            }
        }

        private async Task<Comments> CommentExists(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                throw new NotFoundException($"Comment with id {id} not found.");
            }

            return comment;
        }

        private async Task IsCommentAuthor(int commentUserId, int userId)
        {
            if (commentUserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action.");
            }
        }
    }
}