using Kmm.AspNetCore.FeatureFlag.Abstractions;

namespace Kmm.OrderService.Web.Endpoints;

public static class CheckoutEndpoints
{
    public static IEndpointRouteBuilder MapCheckoutEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/checkouts")
            .WithTags("Checkout")
            .RequireAuthorization();

        group.MapPost("", async (
            IFeatureFlagClient flags,
            CancellationToken cancellationToken) =>
        {
            var context = new FeatureFlagContext();
            var enabled = await flags.IsEnabledAsync("release.checkout.v3", context, cancellationToken);

            return enabled
                ? Results.Ok(new { flow = "v3" })
                : Results.Ok(new { flow = "v1" });
        });

        /*
        group.MapPost("/debug", async (
            IUser user,
            IFeatureFlagClient flags,
            CancellationToken cancellationToken) =>
        {
            var context = new FeatureFlagContext();
            var decision = await flags.IsEnabledAsync("release.checkout.v2", context, cancellationToken);

            return decision.Enabled
                ? Results.Ok(new { flow = "v2" })
                : Results.Ok(new { flow = "v1" });
        });
        */

        return app;
    }
}
