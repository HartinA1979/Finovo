using Finovo.Application.Common.Interfaces;
using Finovo.Application.Features.Auth.DTOs;
using Finovo.Domain.Exceptions;
using MediatR;

namespace Finovo.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userId = await _identityService.ValidateCredentialsAsync(request.Email, request.Password);
        if (userId is null)
            throw new DomainException("Invalid email or password.");

        var user = await _identityService.GetUserInfoAsync(userId)
            ?? throw new DomainException("User not found.");

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, user.TenantId, user.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _identityService.StoreRefreshTokenAsync(userId, refreshToken);

        return new AuthResult(accessToken, refreshToken, user.Id, user.Email, user.Role, user.TenantId, user.FirstName, user.LastName);
    }
}
