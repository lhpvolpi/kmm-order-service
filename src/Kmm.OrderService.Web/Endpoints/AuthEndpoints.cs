using Kmm.OrderService.Application.Authentications.CreateToken.Commands;
using Kmm.OrderService.Application.Authentications.CreateToken.Dtos;

namespace Kmm.OrderService.Web.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Auth");

        group.MapPost("/token",
            async (CreateTokenCommand command, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(command, ct);
                return Results.Ok(result);
            })
            .AllowAnonymous()
            .WithName("CreateToken")
            .Produces<CreateTokenDto>(StatusCodes.Status200OK);

        return app;
    }
}

