using Kmm.OrderService.Application.Products.ListProduct.Queries;
using Kmm.OrderService.Application.Products.Shared.Dtos;

namespace Kmm.OrderService.Web.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products")
            .WithTags("Products")
            .RequireAuthorization();

        group.MapGet("",
                async (string? search, ISender sender, CancellationToken ct) =>
                {
                    var result = await sender.Send(new ListProductQuery(search), ct);
                    return Results.Ok(result);
                })
            .WithName("ListProducts")
            .Produces<List<ProductDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        return app;
    }
}
