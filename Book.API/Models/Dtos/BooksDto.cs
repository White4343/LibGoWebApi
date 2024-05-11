namespace Book.API.Models.Dtos
{
    public class BooksDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? PhotoUrl { get; set; }
        public int UserId { get; set; }
    }
}
