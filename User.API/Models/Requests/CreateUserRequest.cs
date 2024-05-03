namespace User.API.Models.Requests
{
    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
    }
}