using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validation.FluentValidation
{
    public class UpdatePostDtoValidator : AbstractValidator<UpdatePostDto>
    {
        public UpdatePostDtoValidator()
        {
            RuleFor(p => p.PostTitle)
                .Length(1, 40).WithMessage("Title length cannot be more than 40 lettera or signs")
                .NotEmpty().WithMessage("You cannot send Title empty");
            RuleFor(p => p.PostContext)
                .Length(1, 1000).WithMessage("Max. length is 1000 letters or sign")
                .NotEmpty().WithMessage("Context cannot be send empty");
        }
    }
}
