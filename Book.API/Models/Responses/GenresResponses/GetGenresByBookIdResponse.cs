using Book.API.Data.Entities;

namespace Book.API.Models.Responses.GenresResponses
{
    public class GetGenresByBookIdResponse
    {
        public int BookId { get; set; }
        public IEnumerable<Genres> Genres { get; set; }
    }
}