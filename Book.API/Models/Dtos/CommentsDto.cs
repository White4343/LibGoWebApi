using System.ComponentModel.DataAnnotations;

namespace Book.API.Models.Dtos
{
    public class CommentsDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}