namespace Kmm.OrderService.Domain.Common.Exceptions;

public sealed class BusinessValidationException : Exception
{
    private const string DefaultErrorMessage = "Invalid entity.";

    public BusinessValidationException(string message = DefaultErrorMessage) : base(message) { }
}

