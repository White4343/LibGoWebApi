namespace User.API.Models.Dtos
{
    public class UserPatchDto
    {
        public string Nickname { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
    }
}