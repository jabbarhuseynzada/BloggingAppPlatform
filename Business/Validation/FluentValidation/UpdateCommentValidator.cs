using Entities.DTOs;
using FluentValidation;

namespace Business.Validation.FluentValidation
{
    public class UpdateCommentValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentValidator()
        {
            RuleFor(c => c.CommentText)
                .Length(1, 500).WithMessage("Maximum comment length is 500")
                .NotEmpty().WithMessage("You cannot send empty comment");
        }
    }
}
