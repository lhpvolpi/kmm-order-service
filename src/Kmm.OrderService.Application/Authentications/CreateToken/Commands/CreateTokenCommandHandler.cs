using Kmm.OrderService.Application.Authentications.CreateToken.Dtos;
using Kmm.OrderService.Application.Common.Auth;

namespace Kmm.OrderService.Application.Authentications.CreateToken.Commands;

public sealed class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, CreateTokenDto>
{
    private readonly IJwtTokenService _jwtTokenService;

    public CreateTokenCommandHandler(IJwtTokenService jwtTokenService)
        => _jwtTokenService = jwtTokenService;

    public Task<CreateTokenDto> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var token = _jwtTokenService.CreateToken(request.Email);
        return Task.FromResult(new CreateTokenDto(token.CustomerId, token.Token));
    }
}

