using System.ComponentModel.DataAnnotations;

namespace Genre.API.Models.Requests
{
    public class CreateBookGenresRequest
    {
        [Required]
        public int BookId { get; set; }
        
        [Required]
        public int GenreId { get; set; }
    }
}