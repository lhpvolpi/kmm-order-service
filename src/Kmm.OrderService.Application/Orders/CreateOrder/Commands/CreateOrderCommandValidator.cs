namespace Kmm.OrderService.Application.Orders.CreateOrder.Commands;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(i => i.CustomerId)
            .NotEmpty()
                .WithMessage("CustomerId is required.");

        RuleFor(i => i.Currency)
            .NotEmpty()
                .WithMessage("Currency is required.")
            .Length(3)
                .WithMessage("Currency must be 3 characters.");

        RuleFor(i => i.Items)
            .NotEmpty()
                .WithMessage("Items is required.")
            .Must(items => items.Select(x => x.ProductId).Distinct().Count() == items.Count)
                .WithMessage("Items cannot contain duplicated ProductId.");

        RuleForEach(i => i.Items)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .NotEmpty()
                        .WithMessage("Product is required.");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                        .WithMessage("Quantity must be greater than zero.");
            });


    }
}

