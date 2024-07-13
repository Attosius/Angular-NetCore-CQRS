using FluentValidation;
using MediatR;

namespace PromomashInc.Server.Handlers;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }


    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request); // Check out the other methods for more advanced handling of validation errors 
        return next();
    }
}