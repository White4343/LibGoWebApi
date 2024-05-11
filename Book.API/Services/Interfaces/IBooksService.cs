﻿using Book.API.Data.Entities;
using Book.API.Models.Requests.BooksRequests;
using Book.API.Models.Responses.BooksResponses;
using Book.API.Models.Responses.GenresResponses;

namespace Book.API.Services.Interfaces
{
    public interface IBooksService
    {
        Task<Books> CreateBookAsync(CreateBooksRequest book, int userId);
        Task<Books> GetBookByIdAsync(int id, int userId);
        Task<GetBookByPageResponse> GetBookPageByIdAsync(int bookId, int userId);
        Task<GetBooksByGenreResponse> GetGenreBooksPageByIdAsync(int genreId);
        Task<IEnumerable<Books>> GetBooksByUserIdAsync(int id, int userId);
        Task<IEnumerable<Books>> GetBooksAsync();
        Task<Books> UpdateBookAsync(UpdateBooksRequest book, int userId);
        Task<bool> DeleteBookAsync(int id, int userId);
    }
}