namespace Genre.API.Models.Responses
{
    public class GetBooksByGenreIdResponse
    {
        public Genres Genre { get; set; }
        public IEnumerable<BookGenres> Books { get; set; }
    }
}
