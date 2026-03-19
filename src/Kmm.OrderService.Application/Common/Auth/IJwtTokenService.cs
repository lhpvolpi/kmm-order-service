namespace Kmm.OrderService.Application.Common.Auth;

public interface IJwtTokenService
{
    JwtToken CreateToken(string email);
}

public sealed record JwtToken(Guid CustomerId, string Token);
