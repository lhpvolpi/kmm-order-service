namespace Kmm.OrderService.Application.Common.Entities;

public interface IUser
{
    Guid CustomerId { get; }

    string Email { get; }
}
