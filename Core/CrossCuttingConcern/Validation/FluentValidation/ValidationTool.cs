
using Core.Entities.Abstract;
using FluentValidation;

namespace Core.CrossCuttingConcern.Validation.FluentValidation
{
    public static class ValidationTool<T>
    {
        public static void Validation(IValidator validator, T entity)
        {
            var validationContext = new ValidationContext<T>(entity);
            var result = validator.Validate(validationContext);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}