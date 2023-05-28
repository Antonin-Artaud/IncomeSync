using FluentValidation;
using MediatR;

namespace IncomeSync.Api.Validations;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var ctx = new ValidationContext<TRequest>(request);

        var failuresEnumerable = _validators
            .Select(v => v.Validate(ctx))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failuresEnumerable.Count != 0)
        {
            throw new ValidationException(failuresEnumerable);
        }

        return next();
    }
}