namespace Admin.API.Data.Entities
{
    public class Comments
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string UserNickname { get; set; }

        public string? UserPhotoUrl { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int UserId { get; set; }
        
        public int BookId { get; set; }
    }
}