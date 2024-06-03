﻿using System.ComponentModel.DataAnnotations;

namespace Book.API.Models.Requests.BooksRequests
{
    public class UpdateBooksRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; } = "";

        public int? Price { get; set; } = 0;

        public DateTime PublishDate { get; set; } = DateTime.Now;

        public string? PhotoUrl { get; set; }

        [Required]
        public bool IsVisible { get; set; } = true;

        public int[]? CoAuthorIds { get; set; }
    }
}
