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

        public int? Price { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.Now;

        public string? PhotoUrl { get; set; }

        [Required]
        public bool IsVisible { get; set; } = true;

        [Required]
        public bool IsAvailableToBuy { get; set; }

        public int[]? CoAuthorIds { get; set; }
    }
}
