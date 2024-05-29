using System.ComponentModel.DataAnnotations;

namespace Book.API.Data.Entities
{
    public class Genres
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
