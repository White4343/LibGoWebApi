using Book.API.Data.Entities;

namespace Book.API.Models.Responses.BooksResponses
{
    public class GetBooksByGenreResponse
    {
        public Genres Genre { get; set; }
        public IEnumerable<Books> Books { get; set; }
    }
}