namespace Book.API.Models.Requests.ReadersRequests
{
    public class CreateReadersRequest
    {
        public string Status { get; set; }
        public int BookId { get; set; }
        public int ChapterId { get; set; }
    }
}
