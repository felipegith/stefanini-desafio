using ErrorOr;
using FluentValidation;
using MediatR;

namespace Stefanini.Application.Middleware;

public class ValidationMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationMiddleware(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (_validator is null || validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors
            .ConvertAll(validation => Error.Validation(validation.PropertyName, validation.ErrorMessage));
        
        return (dynamic)errors;
    }
}