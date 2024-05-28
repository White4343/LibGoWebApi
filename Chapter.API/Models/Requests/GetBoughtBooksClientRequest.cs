namespace Chapter.API.Models.Requests
{
    public class GetBoughtBooksClientRequest
    {
        public long Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Price { get; set; }
        public int SubscriptionId { get; set; }
        public long AuthorUserId { get; set; }
        public long UserId { get; set; }
        public long BookId { get; set; }
    }
}
