using Kmm.OrderService.Application.Common.Entities;

namespace Kmm.OrderService.Infrastructure.Common.Identity;

public sealed class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public Guid CustomerId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(value, out var id) ? id : Guid.Empty;
        }
    }

    public string Email
        => _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(i => i.Type == ClaimTypes.Email)?.Value
        ?? string.Empty;
}
