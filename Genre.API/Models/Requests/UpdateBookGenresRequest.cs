using System.ComponentModel.DataAnnotations;

namespace Genre.API.Models.Requests
{
    public class UpdateBookGenresRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int GenreId { get; set; }
    }
}