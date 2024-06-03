namespace Book.API.Data.Entities
{
    public class Readers
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public bool NotifyEnabled { get; set; }
        public bool IsVisible { get; set; }
        public int UserId { get; set; }
        
        // TODO: Add ratings 1=>5

        // TODO: Add book name and photo
        public int BookId { get; set; }
        public Books Book { get; set; }

        public int? ChapterId { get; set; }
    }
}