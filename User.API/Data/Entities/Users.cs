namespace User.API.Data.Entities
{
    public class Users
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Description { get; set; }
    }
}