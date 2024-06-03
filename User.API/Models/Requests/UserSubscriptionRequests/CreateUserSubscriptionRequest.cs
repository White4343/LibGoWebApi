namespace User.API.Models.Requests.UserSubscriptionRequests
{
    public class CreateUserSubscriptionRequest
    {
        public int SubscriptionId { get; set; }

        public int UserId { get; set; }
    }
}
