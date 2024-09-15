using Castle.DynamicProxy;
using Core.CrossCuttingConcern.Validation.FluentValidation;
using Core.Helpers.Interceptors;
using Core.Entities.Abstract;
using FluentValidation;

namespace Core.Aspects.Autofac.Validation.FluentValidation;

public class ValidationAspect<T> : MethodInterception
{
    private readonly Type _validatorType;

    public ValidationAspect(Type type)
    {
        if (!typeof(IValidator).IsAssignableFrom(type))
        {
            throw new Exception("this class is not a validation class");
        }
        _validatorType = type;
    }

    protected override void OnBefore(IInvocation invocation)
    {

        var validator = (IValidator)Activator.CreateInstance(_validatorType);
        var entityType = _validatorType.BaseType.GetGenericArguments()[0];
        var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
        foreach (T entity in entities)
        {
            ValidationTool<T>.Validation(validator, entity);
        }
    }
}
