using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
