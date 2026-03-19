namespace Kmm.OrderService.Application.Authentications.CreateToken.Dtos;

public sealed record CreateTokenDto(Guid CustomerId, string Token);
