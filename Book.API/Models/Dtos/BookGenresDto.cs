namespace Book.API.Models.Dtos
{
    public class BookGenresDto
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int GenreId { get; set; }
    }
}
