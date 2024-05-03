using FluentValidation;
using FluentValidation.Validators;
using User.API.Data.Entities;
using User.API.Models.Requests;
using User.API.Repositories.Interfaces;

namespace User.API.Validation
{
    public class UserValidator : AbstractValidator<Users>
    {
        private readonly IUserValidationRepository _userRepository;

        public UserValidator(IUserValidationRepository userRepository)
        {
            _userRepository = userRepository;
            
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Login is required")
                .MinimumLength(6).WithMessage("Login can't be shorter than 6 characters")
                .MaximumLength(20).WithMessage("Login can't be longer than 20 characters")
                .Matches(@"[A-Za-z0-9]+").WithMessage("Login can only contain letters and numbers")
                .MustAsync(IsLoginUnique).WithMessage("Login is already taken");
            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password can't be shorter than 6 characters")
                .MaximumLength(20).WithMessage("Password can't be longer than 20 characters")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"^(?:(?![\!\?\*\.]).)*$").WithMessage("Your password must NOT contain at least one (!? *.)");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid")
                .MaximumLength(50).WithMessage("Email can't be longer than 50 characters")
                .MustAsync(IsEmailUnique).WithMessage("Email is already taken");
            RuleFor(x => x.Nickname)
                .NotEmpty().WithMessage("Nickname is required")
                .MinimumLength(3).WithMessage("Nickname can't be shorter than 3 characters")
                .MaximumLength(20).WithMessage("Nickname can't be longer than 20 characters");
            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Description can't be longer than 200 characters");
            RuleFor(x => x.PhotoUrl)
                .MaximumLength(200).WithMessage("PhotoUrl can't be longer than 200 characters");
        }

        private async Task<bool> IsLoginUnique(string login, CancellationToken token)
        {
            try
            {
                await _userRepository.UserLoginExists(login);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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
