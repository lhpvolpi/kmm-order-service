using Kmm.OrderService.Application.Common.Exceptions;
using Kmm.OrderService.Domain.Common.Exceptions;

namespace Kmm.OrderService.Web.Common;

public sealed class CustomExceptionHandler : IExceptionHandler
{
    private readonly IHostEnvironment _environment;

    public CustomExceptionHandler(IHostEnvironment environment)
        => _environment = environment;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case ValidationException validationException:
                await HandleValidationException(httpContext, validationException, cancellationToken);
                return true;

            case NotFoundException notFoundException:
                await HandleNotFoundException(httpContext, notFoundException, cancellationToken);
                return true;

            case UnauthorizedAccessException:
                await HandleUnauthorizedAccessException(httpContext, cancellationToken);
                return true;

            case ForbiddenAccessException:
                await HandleForbiddenAccessException(httpContext, cancellationToken);
                return true;

            case BusinessValidationException validationException:
                await HandleBusinessValidationException(httpContext, validationException, cancellationToken);
                return true;

            default:
                await HandleUnknownException(httpContext, exception, cancellationToken);
                return true;
        }
    }

    private static string ToCamelCase(string value)
        => string.IsNullOrEmpty(value) ? value : char.ToLowerInvariant(value[0]) + value[1..];

    private static async Task HandleValidationException(
        HttpContext httpContext,
        ValidationException exception,
        CancellationToken cancellationToken)
    {
        var errors = exception.Errors.ToDictionary(
            i => ToCamelCase(i.Key),
            i => i.Value);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation failed.",
                Instance = httpContext.Request.Path
            },
            cancellationToken);
    }

    private static async Task HandleNotFoundException(
        HttpContext httpContext,
        NotFoundException exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            },
            cancellationToken);
    }

    private static async Task HandleUnauthorizedAccessException(
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                Title = "Authentication required.",
                Instance = httpContext.Request.Path
            },
            cancellationToken);
    }

    private static async Task HandleForbiddenAccessException(
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                Title = "Access to this resource is forbidden.",
                Instance = httpContext.Request.Path
            },
            cancellationToken);
    }

    private static async Task HandleBusinessValidationException(
        HttpContext httpContext,
        BusinessValidationException exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Business validation failed.",
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            },
            cancellationToken);
    }

    private async Task HandleUnknownException(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "An unexpected error occurred.",
                Detail = _environment.IsDevelopment() ? exception.Message : null,
                Instance = httpContext.Request.Path
            },
            cancellationToken);
    }
}
