using Core.Entities.Concrete;
using FluentValidation;

namespace Business.Validation.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(1, 30).WithMessage("Username must be between 1 and 30 characters.")
                .Matches(@"^[a-z0-9_\.]+$").WithMessage("Username can only contain lowercase letters, numbers, underscores, and periods.")
                .Matches(@"^(?!.*\.\.)(?!.*\.$)[a-z0-9_.]+(?<!\.)$").WithMessage("Username cannot start or end with a period, and cannot have consecutive periods.");
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Invalid email format. Must contain '@' and a valid domain.")
                .Matches(@"^[\w\.\-]+@(gmail|yahoo|outlook)\.com$")
                .WithMessage("Email must be from a valid provider (e.g., gmail.com, yahoo.com, outlook.com).");

            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First name cannot be empty.")
                .Length(1, 20).WithMessage("First name must be between 1 and 20 characters.")
                .Matches(@"^[a-zA-Z]+$").WithMessage("First name can only contain letters.")
                .Must(name => name.Trim().Equals(name)).WithMessage("First name cannot start or end with spaces.")
                .Must(name => !name.Contains("  ")).WithMessage("First name cannot contain consecutive spaces.");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last name cannot be empty.")
                .Length(1, 20).WithMessage("Last name must be between 1 and 20 characters.")
                .Matches(@"^[a-zA-Z]+$").WithMessage("Last name can only contain letters.")
                .Must(name => name.Trim().Equals(name)).WithMessage("Last name cannot start or end with spaces.")
                .Must(name => !name.Contains("  ")).WithMessage("Last name cannot contain consecutive spaces.");

        }
    }
}
