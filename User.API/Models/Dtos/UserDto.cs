namespace User.API.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Nickname { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}