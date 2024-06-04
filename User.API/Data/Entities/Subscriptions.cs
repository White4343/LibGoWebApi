namespace User.API.Data.Entities
{
    // TODO: Migrate ALL Id to long?
    public class Subscriptions
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int[] BookIds { get; set; }

        public int UserId { get; set; }
    }
}