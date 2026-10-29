using Finovo.Application.Common.Interfaces;
using Finovo.Application.Features.Auth.DTOs;
using Finovo.Domain.Exceptions;
using MediatR;

namespace Finovo.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResult>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var stored = await _identityService.GetRefreshTokenAsync(request.Token);

        if (stored is null || stored.IsRevoked || stored.ExpiresAt < DateTime.UtcNow)
            throw new DomainException("Refresh token is invalid or expired.");

        var user = await _identityService.GetUserInfoAsync(stored.UserId)
            ?? throw new DomainException("User not found.");

        await _identityService.RevokeRefreshTokenAsync(request.Token);

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.TenantId, user.Role);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        await _identityService.StoreRefreshTokenAsync(user.Id, newRefreshToken);

        return new AuthResult(accessToken, newRefreshToken, user.Id, user.Email, user.Role, user.TenantId, user.FirstName, user.LastName);
    }
}
