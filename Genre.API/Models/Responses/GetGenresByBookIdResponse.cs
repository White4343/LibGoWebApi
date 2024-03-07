namespace Genre.API.Models.Responses
{
    public class GetGenresByBookIdResponse
    {
        public int BookId { get; set; }
        public IEnumerable<Genres> Genres { get; set; }
    }
}