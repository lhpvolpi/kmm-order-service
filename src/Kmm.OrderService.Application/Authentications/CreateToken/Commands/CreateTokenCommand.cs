using System.ComponentModel.DataAnnotations;
using Kmm.OrderService.Application.Authentications.CreateToken.Dtos;

namespace Kmm.OrderService.Application.Authentications.CreateToken.Commands;

public sealed record CreateTokenCommand : IRequest<CreateTokenDto>
{
    [Required]
    public string Email { get; set; } = default!;
}

