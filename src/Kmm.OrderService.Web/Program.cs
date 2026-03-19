using Kmm.OrderService.Application;
using Kmm.OrderService.Infrastructure;
using Kmm.OrderService.Web;
using Kmm.OrderService.Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
    await app.InitializeDatabaseAsync();
    await app.CreateSeedAsync(cancellationToken: default);
}
else
{
    // app.UseExceptionHandler();
    app.UseHsts();
}

// middlewares “infra”
app.UseHealthChecks("/health");
app.UseHttpsRedirection();

// swagger (público)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kmm Order Service API v1");
    c.RoutePrefix = "swagger";
});

app.UseRouting();

// auth
app.UseAuthentication();
app.UseAuthorization();

// endpoints
app.MapAuthEndpoints();
app.MapCheckoutEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();

app.Run();

public partial class Program { }
