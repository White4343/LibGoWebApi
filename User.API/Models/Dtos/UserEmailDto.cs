using User.API.Data.Entities;

namespace User.API.Models.Dtos
{
    public class UserEmailDto : Users
    {
        public string Email { get; set; }
    }
}
