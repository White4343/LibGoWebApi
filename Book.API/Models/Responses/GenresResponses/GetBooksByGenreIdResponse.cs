using Book.API.Data.Entities;

namespace Book.API.Models.Responses.GenresResponses
{
    public class GetBooksByGenreIdResponse
    {
        public Genres Genre { get; set; }
        public IEnumerable<BookGenres> Books { get; set; }
    }
}
