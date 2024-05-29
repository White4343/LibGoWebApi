﻿using Book.API.Data.Entities;

namespace Book.API.Models.Responses.BooksResponses
{
    public class GetAllBooksWithGenreNamesResponse
    {
        public Books Book { get; set; }
        public IEnumerable<Genres> Genres { get; set; }
    }
}