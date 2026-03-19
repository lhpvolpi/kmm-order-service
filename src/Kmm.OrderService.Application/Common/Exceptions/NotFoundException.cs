namespace Kmm.OrderService.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    private const string DefaultErrorMessage = "Not found.";

    public NotFoundException(string message = DefaultErrorMessage) : base(message) { }

    public static void ThrowIfNull<T>(T? entity, string message = DefaultErrorMessage)
    {
        if (entity is null)
        {
            throw new NotFoundException(message);
        }
    }

    public static void ThrowIfNull<T>(List<T>? entities, string message = DefaultErrorMessage)
    {
        if (entities is null)
        {
            throw new NotFoundException(message);
        }
    }
}

