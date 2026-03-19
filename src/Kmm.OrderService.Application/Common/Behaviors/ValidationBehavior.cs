using ValidationException = Kmm.OrderService.Application.Common.Exceptions.ValidationException;

namespace Kmm.OrderService.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            _logger.LogInformation("Running validation for {RequestType}", typeof(TRequest).Name);

            var context = new ValidationContext<TRequest>(request);
            var results = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = results
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count > 0)
            {
                _logger.LogWarning(
                    "Validation failed for {RequestType}. Errors: {@Errors}",
                    typeof(TRequest).Name,
                    failures.Select(f => new { f.PropertyName, f.ErrorMessage }));

                throw new ValidationException(failures);
            }
        }

        return await next(cancellationToken);
    }
}
