using AutoMapper;
using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests;
using Book.API.Repositories.Interfaces;
using Book.API.Services.Interfaces;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IBooksService _booksService;
        private readonly ILogger<CommentsService> _logger;
        private readonly IMapper _mapper;

        public CommentsService(ICommentsRepository commentsRepository, IBooksService booksService,
            ILogger<CommentsService> logger, IMapper mapper)
        {
            _commentsRepository = commentsRepository;
            _booksService = booksService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CommentsDto> CreateCommentAsync(CreateCommentsRequest comment, int userId)
        {
            try
            {
                await BookExists(comment.BookId);

                var commentToCreate = new Comments
                {
                    Content = comment.Content,
                    BookId = comment.BookId,
                    UserId = userId
                };

                var createdComment = await _commentsRepository.CreateCommentAsync(commentToCreate);

                return _mapper.Map<CommentsDto>(createdComment);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CommentsDto> GetCommentByIdAsync(int id)
        {
            try
            {
                var comment = await _commentsRepository.GetCommentByIdAsync(id);

                return _mapper.Map<CommentsDto>(comment);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<CommentsDto>> GetCommentsByBookIdAsync(int bookId)
        {
            try
            {
                var comments = await _commentsRepository.GetCommentsByBookIdAsync(bookId);

                return _mapper.Map<IEnumerable<CommentsDto>>(comments);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<CommentsDto>> GetCommentsByUserIdAsync(int userId)
        {
            try
            {
                var comments = await _commentsRepository.GetCommentsByUserIdAsync(userId);

                return _mapper.Map<IEnumerable<CommentsDto>>(comments);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CommentsDto> UpdateCommentAsync(UpdateCommentsRequest comment, int userId)
        {
            try
            {
                await BookExists(comment.BookId);

                var commentToUpdate = new Comments
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    BookId = comment.BookId,
                    UserId = userId
                };

                var updatedComment = await _commentsRepository.UpdateCommentAsync(commentToUpdate);

                return _mapper.Map<CommentsDto>(updatedComment);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteCommentAsync(int id, int userId)
        {
            try
            {
                var comment = await _commentsRepository.DeleteCommentAsync(id, userId);

                return comment;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteCommentsByBookIdAsync(int bookId, int userId)
        {
            try
            {
                var deleted = await _commentsRepository.DeleteCommentsByBookIdAsync(bookId, userId);

                return deleted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteCommentsByUserIdAsync(int userId)
        {
            try
            {
                var deleted = await _commentsRepository.DeleteCommentsByUserIdAsync(userId);

                return deleted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Books> BookExists(int id)
        {
            try
            {
                var book = await _booksService.GetBookByIdAsync(id);

                return book;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}