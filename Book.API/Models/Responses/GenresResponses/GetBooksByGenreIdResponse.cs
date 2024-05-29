using Book.API.Data.Entities;
using Book.API.Models.Dtos;

namespace Book.API.Models.Responses.GenresResponses
{
    public class GetBooksByGenreIdResponse
    {
        public Genres Genre { get; set; }
        public IEnumerable<BookGenresDto> Books { get; set; }
    }
}
