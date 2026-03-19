namespace Kmm.OrderService.Application.Products.ListProduct.Queries;

public sealed class ListProductQueryValidator : AbstractValidator<ListProductQuery>
{
    public ListProductQueryValidator()
    {
        When(i => !string.IsNullOrEmpty(i.Search), () =>
        {
            RuleFor(i => i.Search)
                .Length(3)
                    .WithMessage("Search must be 3 characters.");
        });
    }
}

