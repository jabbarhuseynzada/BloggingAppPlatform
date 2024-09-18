using Core.Entities.Concrete;
using FluentValidation;

namespace Business.Validation.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Email should be contains @, and endings like gmail.com etc.")
                .NotEmpty().WithMessage("Email cannot be send empty");
            RuleFor(u => u.FirstName)
                .Length(1, 20).WithMessage("First name's max length is 20").NotEmpty()
                .WithMessage("You cannot send firstname empty");
            RuleFor(u => u.LastName)
                .Length(1, 20).WithMessage("Lastname's max length is 20").NotEmpty()
                .WithMessage("You cannot send lastname empty");
        }
    }
}
