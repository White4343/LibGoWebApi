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
        public string UserNickname { get; set; }

        public string? UserPhotoUrl { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}