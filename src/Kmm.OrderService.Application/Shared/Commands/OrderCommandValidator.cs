using Kmm.OrderService.Application.Common.Validators;

namespace Kmm.OrderService.Application.Shared.Commands;

public class OrderCommandValidator<T> : AbstractValidator<T> where T : OrderCommand
{
    public OrderCommandValidator()
    {
        RuleFor(i => i.Id)
            .IdValidator("Order id is required.");
    }
}

