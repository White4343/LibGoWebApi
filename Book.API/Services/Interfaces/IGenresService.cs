﻿using Book.API.Data.Entities;
using Book.API.Models.Responses;
using Book.API.Models.Responses.GenresResponses;

namespace Book.API.Services.Interfaces
{
    public interface IGenresService
    {
        Task<Genres> CreateGenreAsync(Genres request);
        Task<Genres> GetGenreByIdAsync(int id);
        Task<IEnumerable<Genres>> GetGenresAsync();
        Task<Genres> UpdateGenreAsync(Genres request);
        Task DeleteGenreAsync(int id);
    }
}