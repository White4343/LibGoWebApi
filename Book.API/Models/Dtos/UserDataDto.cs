namespace Book.API.Models.Dtos
{
    public class UserDataDto
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Nickname { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
