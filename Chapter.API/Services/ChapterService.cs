using Chapter.API.Data.Entities;
using Chapter.API.Models.Requests;
using Chapter.API.Repositories.Interfaces;
using Chapter.API.Services.Interfaces;
using FluentValidation;

namespace Chapter.API.Services
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IBookService _bookService;
        private readonly IValidator<Chapters> _validator;
        private readonly ILogger<ChapterService> _logger;

        public ChapterService(IChapterRepository chapterRepository,  ILogger<ChapterService> logger, IBookService bookService, IValidator<Chapters> validator)
        {
            _chapterRepository = chapterRepository;
            _logger = logger;
            _bookService = bookService;
            _validator = validator;
        }

        public async Task<Chapters> CreateChapterAsync(CreateChapterRequest chapter, int userId)
        {
            try
            {
                var book = await BookExistsAsync(chapter.BookId);

                await IsBookAuthor(book.UserId, userId);

                var chapterToCreate = new Chapters
                {
                    Title = chapter.Title,
                    Content = chapter.Content,
                    IsFree = chapter.IsFree,
                    BookId = chapter.BookId
                };

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

        public async Task<Chapters> GetChapterByIdAsync(int id)
        {
            try
            {
                var chapter = await _chapterRepository.GetChapterByIdAsync(id);

                return chapter;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Chapters>> GetChaptersByBookIdAsync(int bookId)
        {
            try
            {
                var chapters = await _chapterRepository.GetChaptersByBookIdAsync(bookId);

                return chapters;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Chapters> UpdateChapterAsync(UpdateChapterRequest chapter, int userId)
        {
            try
            {
                var book = await BookExistsAsync(chapter.BookId);

                await IsBookAuthor(book.UserId, userId);

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

        public async Task<bool> DeleteChapterAsync(int id, int userId)
        {
            try
            {
                var chapter = await _chapterRepository.GetChapterByIdAsync(id);

                var book = await BookExistsAsync(chapter.BookId);

                await IsBookAuthor(book.UserId, userId);

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

        public async Task<bool> DeleteChaptersByBookIdAsync(int bookId, int userId)
        {
            try
            {
                var book = await BookExistsAsync(bookId);

                await IsBookAuthor(book.UserId, userId);

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

        private async Task<Books> BookExistsAsync(int bookId)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(bookId);

                return book;
            }
            catch (Exception a)
            {
                Console.WriteLine(a);
                throw;
            }
        }

        private async Task<bool> IsBookAuthor(int userBookId, int userId)
        {
            if (userBookId != userId) 
            { 
                throw new UnauthorizedAccessException("You are not authorized to perform this action");
            }

            return true;
        }
    }
}
