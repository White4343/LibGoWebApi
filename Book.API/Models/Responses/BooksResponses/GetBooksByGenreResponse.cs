using Book.API.Data.Entities;
using Book.API.Models.Dtos;

namespace Book.API.Models.Responses.BooksResponses
{
    public class GetBooksByGenreResponse
    {
        public Genres Genre { get; set; }
        public IEnumerable<BooksDto> Books { get; set; }
    }
}