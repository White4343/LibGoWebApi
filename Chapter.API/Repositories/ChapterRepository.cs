using Chapter.API.Data;
using Chapter.API.Data.Entities;
using Chapter.API.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace Chapter.API.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ChapterRepository> _logger;

        public ChapterRepository(AppDbContext context, ILogger<ChapterRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Chapters> CreateChapterAsync(Chapters chapter)
        {
            try
            {
                var chapterToCreate = await _context.Chapters.AddAsync(chapter);

                await _context.SaveChangesAsync();

                return chapterToCreate.Entity;
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
                var chapter = await ChapterExitsAsync(id);

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
                var chapters = await _context.Chapters.Where(c => c.BookId == bookId).ToListAsync();

                if (chapters.Count == 0 || chapters == null)
                {
                    throw new NotFoundException($"Chapters for book with id {bookId} not found");
                }

                return chapters;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Chapters> UpdateChapterAsync(Chapters chapter)
        {
            try
            {
                var chapterToUpdate = await ChapterExitsAsync(chapter.Id);

                if (chapterToUpdate.BookId != chapter.BookId)
                {
                    throw new FormatException($"You can't change bookId {chapter.BookId} to {chapterToUpdate.BookId}");
                }

                chapterToUpdate.Title = chapter.Title;
                chapterToUpdate.Content = chapter.Content;
                chapterToUpdate.IsFree = chapter.IsFree;
                chapterToUpdate.UpdatedAt = DateTime.UtcNow;

                _context.Chapters.Update(chapterToUpdate);

                await _context.SaveChangesAsync();

                return chapterToUpdate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteChapterAsync(int id)
        {
            try
            {
                var chapterToDelete = await ChapterExitsAsync(id);

                _context.Chapters.Remove(chapterToDelete);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteChaptersByBookIdAsync(int bookId)
        {
            try
            {
                var chaptersToDelete = await GetChaptersByBookIdAsync(bookId);

                _context.Chapters.RemoveRange(chaptersToDelete);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Chapters> ChapterExitsAsync(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);

            if (chapter == null)
            {
                throw new NotFoundException($"Chapter with id {id} not found");
            }

            return chapter;
        }
    }
}
