using Kmm.OrderService.Application.Orders.CancelOrder.Commands;
using Kmm.OrderService.Application.Orders.CancelOrder.Dtos;
using Kmm.OrderService.Application.Orders.ConfirmOrder.Commands;
using Kmm.OrderService.Application.Orders.ConfirmOrder.Dtos;
using Kmm.OrderService.Application.Orders.CreateOrder.Commands;
using Kmm.OrderService.Application.Orders.CreateOrder.Dtos;
using Kmm.OrderService.Application.Orders.GetOrder.Queries;
using Kmm.OrderService.Application.Orders.ListOrder.Queries;
using Kmm.OrderService.Application.Shared.Dtos;

namespace Kmm.OrderService.Web.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders")
            .WithTags("Orders")
            .RequireAuthorization();

        // CREATE
        group.MapPost("",
                async (CreateOrderCommand command, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(command, cancellationToken);
                    return Results.Created($"/orders/{result.Id}", result);
                })
            .WithName("CreateOrder")
            .Produces<CreateOrderDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        // GET BY ID
        group.MapGet("{id:guid}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(new GetOrderQuery(id), cancellationToken);
                    return Results.Ok(result);
                })
            .WithName("GetOrder")
            .Produces<OrderDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        // LIST (PAGINATED)
        group.MapGet("",
                async ([AsParameters] ListOrderQuery query, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(query, cancellationToken);
                    return Results.Ok(result);
                })
            .WithName("ListOrders")
            .Produces<PaginatedListDto<OrderDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        // CONFIRM
        group.MapPost("{id:guid}/confirm",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(new ConfirmOrderCommand(id), cancellationToken);
                    return Results.Ok(result);
                })
            .WithName("ConfirmOrder")
            .Produces<ConfirmOrderDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        // CANCEL
        group.MapPost("{id:guid}/cancel",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(new CancelOrderCommand(id), cancellationToken);
                    return Results.Ok(result);
                })
            .WithName("CancelOrder")
            .Produces<CancelOrderDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        return app;
    }
}
