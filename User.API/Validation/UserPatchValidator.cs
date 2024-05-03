using FluentValidation;
using User.API.Models.Dtos;

namespace User.API.Validation
{
    public class UserPatchValidator : AbstractValidator<UserPatchDto>
    {
        public UserPatchValidator()
        {
            RuleFor(x => x.Nickname)
                .MinimumLength(6).WithMessage("Nickname can't be shorter than 3 characters")
                .MaximumLength(20).WithMessage("Nickname can't be longer than 20 characters");
            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Description can't be longer than 200 characters");
            RuleFor(x => x.PhotoUrl)
                .MaximumLength(200).WithMessage("PhotoUrl can't be longer than 200 characters")
                .Matches(@"(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|gif|png)").WithMessage("PhotoUrl must be a valid URL to an image");
        }
    }
}
