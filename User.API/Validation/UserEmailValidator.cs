using FluentValidation;
using User.API.Data.Entities;
using User.API.Models.Dtos;
using User.API.Repositories.Interfaces;

namespace User.API.Validation
{
    public class UserEmailValidator : AbstractValidator<UserEmailDto>
    {
        private readonly IUserValidationRepository _userRepository;

        public UserEmailValidator(IUserValidationRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid")
                .MaximumLength(50).WithMessage("Email can't be longer than 50 characters")
                .MustAsync(IsEmailUnique).WithMessage("Email is already taken");
        }

        private async Task<bool> IsEmailUnique(string email, CancellationToken token)
        {
            try
            {
                await _userRepository.UserEmailExists(email);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
