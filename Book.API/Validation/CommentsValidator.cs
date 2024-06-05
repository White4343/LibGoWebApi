using Book.API.Data.Entities;
using Book.API.Models.Requests;
using FluentValidation;


namespace Book.API.Validation
{
    public class CommentsValidator : AbstractValidator<Comments>
    {
        // TODO: Add validation for bad words
        public CommentsValidator()
        {
            RuleFor(x => x.Content).NotNull().WithMessage("Content is required")
                .MaximumLength(200).WithMessage("Content can't be longer than 200 characters");
            RuleFor(x => x.BookId).NotNull().WithMessage("BookId is required");
        }
    }
}