using System.ComponentModel.DataAnnotations;

namespace Book.API.Data.Entities
{
    public class BookGenres
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }
        public Books Book { get; set; }

        // TODO: Add genre name (low)
        [Required]
        public int GenreId { get; set; }
        public Genres Genre { get; set; }
    }
}