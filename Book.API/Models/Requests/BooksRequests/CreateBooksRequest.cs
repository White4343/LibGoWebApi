﻿using System.ComponentModel.DataAnnotations;

namespace Book.API.Models.Requests.BooksRequests
{
    public class CreateBooksRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        public string? PhotoUrl { get; set; }

        [Required]
        public bool IsVisible { get; set; }

        public int[]? CoAuthorIds { get; set; }
    }
}