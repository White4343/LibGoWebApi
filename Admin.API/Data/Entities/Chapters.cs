namespace Admin.API.Data.Entities
{
    public class Chapters
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsFree { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int AuthorUserId { get; set; }
        public int BookId { get; set; }
    }
}