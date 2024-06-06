using AutoMapper;
using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests.CommentsRequests;
using Book.API.Repositories.Interfaces;
using Book.API.Services.Interfaces;
using Book.API.Validation;
using FluentValidation;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IBooksService _booksService;
        private readonly ILogger<CommentsService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<Comments> _validator;

        public CommentsService(ICommentsRepository commentsRepository, IBooksService booksService,
            ILogger<CommentsService> logger, IMapper mapper, IValidator<Comments> validator)
        {
            _commentsRepository = commentsRepository;
            _booksService = booksService;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CommentsDto> CreateCommentAsync(CreateCommentsRequest comment, UserDataDto user)
        {
            try
            {
                var book = await BookExists(comment.BookId, user.Id);

                if (!book.IsVisible)
                {
                    throw new UnauthorizedAccessException("You can't comment on a book that is not visible.");
                }

                var commentToCreate = new Comments
                {
                    Content = comment.Content,
                    UserNickname = user.Nickname,
                    UserPhotoUrl = user.PhotoUrl,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = null,
                    BookId = comment.BookId,
                    UserId = user.Id
                };

                var validationResult = await _validator.ValidateAsync(commentToCreate);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                commentToCreate.Content = await ValidateCommentContent(commentToCreate.Content);

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
                var book = await BookExists(bookId, -1);

                if (!book.IsVisible)
                {
                    throw new UnauthorizedAccessException("You can't get comments for a book that is not visible.");
                }

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

        public async Task<CommentsDto> UpdateCommentAsync(UpdateCommentsRequest comment, UserDataDto user)
        {
            try
            {
                var book = await BookExists(comment.BookId, user.Id);

                var commentToUpdate = new Comments
                {
                    Id = comment.Id,
                    UserNickname = user.Nickname,
                    UserPhotoUrl = user.PhotoUrl,
                    Content = comment.Content,
                    BookId = comment.BookId,
                    UserId = user.Id
                };

                var validationResult = await _validator.ValidateAsync(commentToUpdate);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                commentToUpdate.Content = await ValidateCommentContent(commentToUpdate.Content);

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
                var comment = await _commentsRepository.GetCommentByIdAsync(id);

                var book = await BookExists(comment.BookId, userId);

                if (userId != book.UserId || comment.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to perform this action.");
                }

                var commentTodDelete = await _commentsRepository.DeleteCommentAsync(id, userId);

                return commentTodDelete;
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
                var book = await BookExists(bookId, userId);

                if (book.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to perform this action.");
                }

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

        private async Task<Books> BookExists(int id, int userId)
        {
            try
            {
                var book = await _booksService.GetBookByIdAsync(id, userId);

                return book;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void IsCommentAuthor(int commentUserId, int userId)
        {
            if (commentUserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action.");
            }
        }

        private async Task<string> ValidateCommentContent(string content)
        {

            var censorValidator = await CensorValidator.CreateFromFileAsync();
            
            var result = censorValidator.CensorText(content);

            return result;
        }
    }
}