using User.API.Data.Entities;

namespace User.API.Repositories.Interfaces
{
    public interface IUserValidationRepository
    {
        Task UserEmailExists(string email);
        Task UserLoginExists(string login);
    }
}
