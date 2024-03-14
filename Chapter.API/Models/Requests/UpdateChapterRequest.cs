using System.ComponentModel.DataAnnotations;

namespace Chapter.API.Models.Requests
{
    public class UpdateChapterRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool IsFree { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}