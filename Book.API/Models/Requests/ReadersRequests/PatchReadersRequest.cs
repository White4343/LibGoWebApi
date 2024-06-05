namespace Book.API.Models.Requests.ReadersRequests
{
    public class PatchReadersRequest
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public bool? NotifyEnabled { get; set; }
        public bool? IsVisible { get; set; }
        public int? Rating { get; set; }
    }
}