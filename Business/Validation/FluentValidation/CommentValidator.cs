using Entities.DTOs;
using FluentValidation;

namespace Business.Validation.FluentValidation
{
    public class CommentValidator : AbstractValidator<CommentDto>
    {
        public CommentValidator()
        {
            RuleFor(c => c.CommentText)
                .Length(1, 500).WithMessage("Maximum comment length is 500")
                .NotEmpty().WithMessage("You cannot send empty comment");
        }
    }
}
