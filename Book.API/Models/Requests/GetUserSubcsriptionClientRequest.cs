namespace Book.API.Models.Requests
{
    public class GetUserSubcsriptionClientRequest
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int PaidPrice { get; set; }

        public int SubscriptionId { get; set; }

        public int UserId { get; set; }

        public int[] BookIds { get; set; }

        public int AuthorUserId { get; set; }
    }
}
