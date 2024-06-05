using Book.API.Data.Entities;

namespace Book.API.Models.Dtos
{
    public class ReadersDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public bool NotifyEnabled { get; set; }
        public bool IsVisible { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int? ChapterId { get; set; }
    }
}