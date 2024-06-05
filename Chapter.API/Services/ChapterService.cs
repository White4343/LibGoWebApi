using AutoMapper;
using Chapter.API.Data.Entities;
using Chapter.API.Models.Requests;
using Chapter.API.Models.Responses;
using Chapter.API.Repositories.Interfaces;
using Chapter.API.Services.Interfaces;
using FluentValidation;
using SendGrid.Helpers.Errors.Model;

namespace Chapter.API.Services
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IBookService _bookService;
        private readonly IValidator<Chapters> _validator;
        private readonly ILogger<ChapterService> _logger;
        private readonly IBoughtBooksService _boughtBooksService;
        private readonly IMapper _mapper;

        public ChapterService(IChapterRepository chapterRepository,  ILogger<ChapterService> logger, IBookService bookService, IValidator<Chapters> validator, IBoughtBooksService boughtBooksService, IMapper mapper)
        {
            _chapterRepository = chapterRepository;
            _logger = logger;
            _bookService = bookService;
            _validator = validator;
            _boughtBooksService = boughtBooksService;
            _mapper = mapper;
        }

        public async Task<Chapters> CreateChapterAsync(CreateChapterRequest chapter, int userId, string? token)
        {
            try
            {
                var book = await BookExistsAsync(chapter.BookId, token);

                IsBookAuthor(book.UserId, userId);

                var chapterToCreate = new Chapters
                {
                    Title = chapter.Title,
                    Content = chapter.Content,
                    CreatedAt = DateTime.UtcNow,
                    IsFree = chapter.IsFree,
                    BookId = chapter.BookId,
                    AuthorUserId = userId
                };

                if (book.Price == 0)
                {
                    chapterToCreate.IsFree = true;
                }

                var validationResult = await _validator.ValidateAsync(chapterToCreate);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var createdChapter = await _chapterRepository.CreateChapterAsync(chapterToCreate);

                return createdChapter;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<Chapters> GetChapterByIdAsync(int id, int? userId, string? token)
        {
            try
            {
                var chapter = await _chapterRepository.GetChapterByIdAsync(id);

                await CheckUser(chapter, userId, token);

                return chapter;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Chapters>> GetChaptersByBookIdAsync(int bookId, int? userId, string? token)
        {
            try
            {
                await BookExistsAsync(bookId, token);

                var chapters = await _chapterRepository.GetChaptersByBookIdAsync(bookId);
                
                if (chapters.Any(ch => !ch.IsFree))
                {
                    try
                    {
                        IsUserAuthorized(userId);

                        await IsBookBought(bookId, userId, token);

                        return chapters;
                    }
                    catch (Exception e)
                    {
                        chapters = chapters.Where(ch => ch.IsFree).ToList();

                        return chapters;
                    }
                }

                return chapters;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<GetChaptersTitlesByBookIdResponse>> GetChaptersTitlesByBookIdAsync(int bookId)
        {
            try
            {
                var chapters = await _chapterRepository.GetChaptersByBookIdAsync(bookId);

                var chaptersTitles = _mapper.Map<IEnumerable<GetChaptersTitlesByBookIdResponse>>(chapters);

                return chaptersTitles;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Chapters> UpdateChapterAsync(UpdateChapterRequest chapter, int userId, string? token)
        {
            try
            {
                var book = await BookExistsAsync(chapter.BookId, token);

                IsBookAuthor(book.UserId, userId);

                var chapterToUpdate = new Chapters
                {
                    Id = chapter.Id,
                    Title = chapter.Title,
                    Content = chapter.Content,
                    IsFree = chapter.IsFree,
                    BookId = chapter.BookId
                };

                var validationResult = await _validator.ValidateAsync(chapterToUpdate);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var updatedChapter = await _chapterRepository.UpdateChapterAsync(chapterToUpdate);

                return updatedChapter;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<bool> DeleteChapterAsync(int id, int userId, string token)
        {
            try
            {
                var chapter = await _chapterRepository.GetChapterByIdAsync(id);

                var book = await BookExistsAsync(chapter.BookId, token);

                IsBookAuthor(book.UserId, userId);

                var deleted = await _chapterRepository.DeleteChapterAsync(id);

                return deleted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<bool> DeleteChaptersByBookIdAsync(int bookId, int userId, string token)
        {
            try
            {
                var book = await BookExistsAsync(bookId, token);

                IsBookAuthor(book.UserId, userId);

                var deleted = await _chapterRepository.DeleteChaptersByBookIdAsync(bookId);

                return deleted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        private async Task<Books> BookExistsAsync(int bookId, string token)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(bookId, token);

                return book;
            }
            catch (Exception a)
            {
                Console.WriteLine(a);
                throw;
            }
        }

        private async Task IsBookBought(int bookId, int? userId, string? token)
        {
            try
            {
                await _boughtBooksService.GetBoughtBooksByUserIdByBookId(bookId, userId, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task IsSubscriptionActive(int userId, int bookId, string? token)
        {
            try
            {
                await _boughtBooksService.GetUserSubscriptionByUserId(userId, bookId, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void IsBookAuthor(int userBookId, int? userId)
        {
            if (userBookId != userId) 
            { 
                throw new UnauthorizedAccessException("You are not authorized to perform this action");
            }
        }

        private async Task CheckUser(Chapters chapters, int? userId, string? token)
        {
            await BookExistsAsync(chapters.BookId, token);

            if (!chapters.IsFree)
            {
                try
                {
                    IsUserAuthorized(userId);

                    IsBookAuthor(chapters.AuthorUserId, userId);
                }
                catch (UnauthorizedAccessException e)
                {
                    try
                    {
                        await IsBookBought(chapters.BookId, userId, token);
                    }
                    catch (Exception exception)
                    {
                        try
                        {
                            await IsSubscriptionActive(Convert.ToInt32(userId), chapters.BookId, token);
                        }
                        catch (Exception e1)
                        {
                            Console.WriteLine(e1);
                            throw;
                        }
                    }
                }
            }
        }

        private void IsUserAuthorized(int? userId)
        {
            if (userId == null || userId == -1)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action");
            }
        }
    }
}
