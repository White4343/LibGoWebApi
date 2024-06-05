using System.Collections;
using AutoMapper;
using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests.ReadersRequests;
using Book.API.Models.Responses.ReadersResponses;
using Book.API.Repositories.Interfaces;
using Book.API.Services.Interfaces;
using Newtonsoft.Json.Linq;
using SendGrid.Helpers.Errors.Model;

namespace Book.API.Services
{
    public class ReadersService : IReadersService
    {
        private readonly IReadersRepository _readersRepository;
        private readonly IBooksService _booksService;
        private readonly IChapterService _chapterService;
        private readonly ILogger<ReadersService> _logger;
        private readonly IMapper _mapper;

        public ReadersService(IReadersRepository readersRepository, IBooksService booksService,
            ILogger<ReadersService> logger,
            IMapper mapper, IChapterService chapterService)
        {
            _readersRepository = readersRepository;
            _booksService = booksService;
            _logger = logger;
            _mapper = mapper;
            _chapterService = chapterService;
        }


        public async Task<ReadersDto> CreateReaderAsync(CreateReadersRequest reader, int tokenUserId)
        {
            try
            {
                await BookExists(reader.BookId, tokenUserId);

                if (reader.ChapterId != null)
                {
                    var chapter = await ChapterExists(reader.ChapterId);
                    IsChapterInBook(chapter.BookId, reader.ChapterId);
                }

                var mappedReader = _mapper.Map<Readers>(reader);

                mappedReader.UserId = tokenUserId;
                mappedReader.Rating = null;

                var createdReader = await _readersRepository.CreateReaderAsync(mappedReader);

                return _mapper.Map<ReadersDto>(createdReader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<GetReadersBookByUserId> GetReadersByUserIdAsync(int id, int tokenUserId)
        {
            try
            {
                var readers = await _readersRepository.GetReadersByUserIdAsync(id);

                var books = new List<BooksDto>();

                foreach (var reader in readers)
                {
                    try
                    {
                        var book = await _booksService.GetBookByIdAsync(reader.BookId, tokenUserId);

                        var rating = reader.Rating != null ? reader.Rating : 0;

                        var mappedBook = _mapper.Map<BooksDto>(book);

                        var ratingDouble = Convert.ToDouble(rating);
                        
                        mappedBook.Rating = ratingDouble;

                        books.Add(mappedBook);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        continue;
                    }
                }

                var readersDto = _mapper.Map<IEnumerable<ReadersDto>>(readers);
                
                if (tokenUserId != id)
                {
                    readersDto = readersDto.Where(x => x.IsVisible).ToList();

                    if (readersDto.Count() == 0)
                    {
                        throw new UnauthorizedAccessException("There are no books in library that you can see");
                    }

                    // filter out books where bookId is not in readersDto
                    books = books.Where(x => readersDto.Any(y => y.BookId == x.Id)).ToList();
                }

                var response = new GetReadersBookByUserId
                {
                    UserId = id,
                    Readers = readersDto,
                    Books = books
                };

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Readers> UpdateReaderAsync(UpdateReadersRequest readerUpdate, int userId)
        {
            try
            {
                CheckRating(readerUpdate.Rating);

                var reader = await ReaderExists(readerUpdate.Id);

                IsReaderAuthor(userId, reader.UserId);

                var chapter = await ChapterExists(readerUpdate.ChapterId);

                IsChapterInBook(chapter.BookId, reader.BookId);

                var updatedReader = await _readersRepository.UpdateReaderAsync(_mapper.Map<Readers>(readerUpdate));

                return updatedReader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Readers> PatchReaderAsync(PatchReadersRequest readerToPatchRequest, int userId)
        {
            try
            {
                if (readerToPatchRequest.Rating != null)
                    CheckRating(Convert.ToInt32(readerToPatchRequest.Rating));
                
                var reader = await ReaderExists(userId);

                IsReaderAuthor(userId, reader.UserId);

                var patchedReader = await _readersRepository.PatchReaderAsync(readerToPatchRequest);

                return patchedReader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Readers> PatchReaderChapterAsync(int readerId, int chapterId, int userId)
        {
            try
            {
                var reader = await ReaderExists(readerId);

                IsReaderAuthor(userId, reader.UserId);

                var chapter = await ChapterExists(chapterId);

                IsChapterInBook(chapter.BookId, reader.BookId);

                var patchedReader = await _readersRepository.PatchReaderChapterIdAsync(readerId, chapterId);

                return patchedReader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteReaderAsync(int id, int userId)
        {
            try
            {
                var reader = await ReaderExists(id);

                IsReaderAuthor(userId, reader.UserId);

                var deleted = await _readersRepository.DeleteReaderAsync(id);

                return deleted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task BookExists(int bookId, int userId)
        {
            try
            {
                var book = await _booksService.GetBookByIdAsync(bookId, userId);

                if (book == null)
                {
                    throw new NotFoundException($"Book with id {bookId} not found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Readers> ReaderBookExists(int readerId, int bookId)
        {
            try
            {
                var reader = await _readersRepository.GetReaderByUserIdAndBookIdAsync(readerId, bookId);

                if (reader == null)
                {
                    throw new NotFoundException($"Reader with id {readerId} not found");
                }

                return reader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Chapters> ChapterExists(int chapterId)
        {
            try
            {
                return await _chapterService.GetChapterByIdAsync(chapterId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<Readers> ReaderExists(int id)
        {
            try
            {
                var reader = await _readersRepository.GetReaderByIdAsync(id);

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

        private void IsReaderAuthor(int userId, int readerUserId)
        {
            if (userId != readerUserId)
            {
                throw new UnauthorizedAccessException("You are not the author of this reader");
            }
        }

        private void IsChapterInBook(int chapterBookId, int bookId)
        {
            if (chapterBookId != bookId)
            {
                throw new UnauthorizedAccessException("Chapter is not part of this book");
            }
        }

        private void CheckRating(int value)
        {
            if (value < 1 || value > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5");
            }
        }
    }
}
