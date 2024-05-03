using FluentValidation;
using User.API.Data.Entities;

namespace User.API.Validation
{
    public class UserPasswordValidator : AbstractValidator<string>
    {
        public UserPasswordValidator()
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password can't be shorter than 6 characters")
                .MaximumLength(20).WithMessage("Password can't be longer than 20 characters")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"^(?:(?![\!\?\*\.]).)*$").WithMessage("Your password must NOT contain at least one (!? *.)");
        }
    }
}