﻿using Chapter.API.Models.Requests;

namespace Chapter.API.Services.Interfaces
{
    public interface IBoughtBooksService
    {
        Task<GetBoughtBooksClientRequest> GetBoughtBooksByUserIdByBookId(int bookId, int? userId, string? token);
    }
}
