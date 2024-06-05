namespace Book.API.Models.Dtos
{
    public class BooksDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsAvailableToBuy { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime PublishDate { get; set; }
        public double Rating { get; set; }
        public int UserId { get; set; }
    }
}