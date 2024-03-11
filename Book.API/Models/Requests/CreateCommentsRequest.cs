using System.ComponentModel.DataAnnotations;

namespace Book.API.Models.Requests
{
    public class CreateCommentsRequest
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}