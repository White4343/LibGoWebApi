namespace Chapter.API.Data.Entities
{
    public class Books
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsVisible { get; set; }
        public bool IsAvailableToBuy { get; set; }
        public int UserId { get; set; }
        public int[]? CoAuthorIds { get; set; }
    }
}