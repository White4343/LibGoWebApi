using Chapter.API.Data.Entities;
using FluentValidation;

namespace Chapter.API.Validations
{
    public class ChapterValidator : AbstractValidator<Chapters>
    {
        public ChapterValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage("Title is required")
                .MaximumLength(50).WithMessage("Title can't be longer than 50 characters");
            RuleFor(x => x.Content).NotNull().WithMessage("Content is required");
        }
    }
}