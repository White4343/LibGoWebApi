namespace User.API.Models.Requests
{
    public class UpdateSubscriptionRequest
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int[] BookIds { get; set; }
    }
}