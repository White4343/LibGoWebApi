using Book.API.Data;
using Book.API.Data.Entities;
using Book.API.Models.Requests.ReadersRequests;
using Book.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Repositories
{
    public class ReadersRepository : IReadersRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ReadersRepository> _logger;

        public ReadersRepository(AppDbContext context, ILogger<ReadersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Readers> CreateReaderAsync(Readers reader)
        {
            try
            {
                var readerToCreate = await _context.Readers.AddAsync(reader);

                _logger.LogInformation($"User with id {reader.UserId} is added to library book with id {reader.BookId}");

                await _context.SaveChangesAsync();

                return readerToCreate.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Readers> GetReaderByIdAsync(int id)
        {
            try
            {
                var reader = await _context.Readers.FindAsync(id);

                if (reader == null)
                {
                    throw new NotFoundException($"Reader with id {id} not found");
                }

                return reader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Readers> GetReaderByUserIdAndBookIdAsync(int userId, int bookId)
        {
            try
            {
                var reader = await _context.Readers.FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);

                if (reader == null)
                {
                    throw new NotFoundException($"Reader with user id {userId} and book id {bookId} not found");
                }

                return reader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Readers>> GetReadersAsync()
        {
            try
            {
                var readers = await _context.Readers.ToListAsync();

                if (readers == null || readers.Count == 0)
                {
                    throw new NotFoundException("No readers found");
                }

                return readers;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Readers>> GetReadersByUserIdAsync(int id)
        {
            try
            {
                var readers = await _context.Readers.Where(r => r.UserId == id).ToListAsync();

                if (readers == null || readers.Count == 0)
                {
                    throw new NotFoundException($"No readers found for user with id {id}");
                }

                return readers;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Readers>> GetReadersByBookIdAsync(int id)
        {
            try
            {
                var readers = await _context.Readers.Where(r => r.BookId == id).ToListAsync();

                if (readers == null || readers.Count == 0)
                {
                    throw new NotFoundException($"No readers found for book with id {id}");
                }

                return readers;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<double> GetBooksRatingByBookIdAsync(int bookId)
        {
            try
            {
                var readers = await GetReadersByBookIdAsync(bookId);

                var ratings = readers.Where(x => x.Rating != null).Select(x => x.Rating).ToList();

                if (ratings.Count == 0)
                {
                    return 0;
                }

                var rating = ratings.Average();

                var result = Convert.ToDouble(rating);

                return result;
            }
            catch (NotFoundException e)
            {
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Readers> UpdateReaderAsync(Readers reader)
        {
            try
            {
                var readerToUpdate = await _context.Readers.FindAsync(reader.Id);

                if (readerToUpdate == null)
                {
                    throw new NotFoundException($"Reader with id {reader.Id} not found");
                }

                readerToUpdate.ChapterId = reader.ChapterId;
                readerToUpdate.IsVisible = reader.IsVisible;
                readerToUpdate.NotifyEnabled = reader.NotifyEnabled;
                readerToUpdate.Status = reader.Status;

                _context.Readers.Update(readerToUpdate);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"User with id {reader.UserId} updated reader with id {reader.Id}");

                return readerToUpdate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Readers> PatchReaderAsync(PatchReadersRequest readersRequest)
        {
            try
            {
                var reader = await _context.Readers.FirstOrDefaultAsync(r => r.Id == readersRequest.Id);

                string field = "";

                if (reader.Status != null)
                {
                    field = "status";
                }

                if (reader.NotifyEnabled != null)
                {
                    field = "notifyEnabled";
                }

                if (reader.IsVisible != null)
                {
                    field = "isVisible";
                }

                switch (field)
                {
                    case "status":
                        switch (readersRequest.Status)
                        {
                            case "Reading":
                                reader.Status = "Reading";
                                break;
                            case "Abandoned":
                                reader.Status = "Abandoned";
                                break;
                            case "Finished":
                                reader.Status = "Finished";
                                break;
                            case "Read Later":
                                reader.Status = "Read Later";
                                break;
                            default:
                                throw new BadRequestException("Invalid status value");
                        }
                        _logger.LogInformation($"User with id {reader.UserId} updated reader status to {reader.Status}");
                        break;
                    case "notifyEnabled":
                        reader.NotifyEnabled = (bool)readersRequest.NotifyEnabled;
                        _logger.LogInformation($"User with id {reader.UserId} updated reader notifyEnabled to {reader.NotifyEnabled}");
                        break;
                    case "isVisible":
                        reader.IsVisible = (bool)readersRequest.IsVisible;
                        _logger.LogInformation($"User with id {reader.UserId} updated reader isVisible to {reader.IsVisible}");
                        break;
                    default:
                        throw new BadRequestException("Invalid field");
                }

                _context.Readers.Update(reader);

                await _context.SaveChangesAsync();

                return reader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Readers> PatchReaderChapterIdAsync(int id, int chapterId)
        {
            try
            {
                var reader = await GetReaderByIdAsync(id);

                reader.ChapterId = chapterId;

                _context.Readers.Update(reader);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Reader with id {id} updated chapter id to {chapterId}");

                return reader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteReaderAsync(int id)
        {
            try
            {
                var reader = await _context.Readers.FindAsync(id);

                if (reader == null)
                {
                    throw new NotFoundException($"Reader with id {id} not found");
                }

                _context.Readers.Remove(reader);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Reader with id {id} deleted");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}