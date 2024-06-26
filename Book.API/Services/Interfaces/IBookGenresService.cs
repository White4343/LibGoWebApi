﻿using Book.API.Data.Entities;
using Book.API.Models;
using Book.API.Models.Dtos;
using Book.API.Models.Requests.BookGenresRequests;
using Book.API.Models.Responses.BooksResponses;

namespace Book.API.Services.Interfaces
{
    public interface IBookGenresService
    {
        Task<BookGenresDto> CreateBookGenreAsync(CreateBookGenresRequests bookGenre, int tokenUserId);
        Task<BookGenresDto> GetBookGenreByIdAsync(int id, int tokenUserId);
        Task<IEnumerable<BookGenresDto>> GetBookGenresByGenreIdAsync(int genreId, int tokenUserId);
        Task<IEnumerable<BookGenresDto>> GetBookGenresByBookIdAsync(int bookId, int tokenUserId);
        Task<GetBookByPageResponse> GetBookPageByIdAsync(int bookId, int userId);
        Task<GetBooksByGenreResponse> GetGenreBooksPageByIdAsync(int genreId, int tokenUserId);
        Task<IEnumerable<GetBooksWithGenreNamesResponse>> GetAllBooksWithGenreNamesAsync(BookFilters? filters);
        Task<IEnumerable<GetBooksWithGenreNamesResponse>> GetBooksByBookNameWithGenreNamesAsync(string name);
        Task<BookGenresDto> UpdateBookGenreAsync(BookGenres bookGenre, int tokenUserId);
        Task DeleteBookGenreAsync(int id, int tokenUserId);
    }
}