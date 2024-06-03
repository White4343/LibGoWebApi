namespace User.API.Data.Entities
{
    public class BoughtBooks
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Price { get; set; }
        public bool IsPaidToAuthor { get; set; }
        public int? SubscriptionId { get; set; }
        public int AuthorUserId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        // TODO: Add book name and photo
    }
}