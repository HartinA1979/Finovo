using Finovo.Application.Common.Interfaces;
using Finovo.Application.Features.Auth.DTOs;
using Finovo.Domain.Exceptions;
using MediatR;

namespace Finovo.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly ITenantContext _tenantContext;

    public RegisterCommandHandler(
        IIdentityService identityService,
        ITokenService tokenService,
        ITenantContext tenantContext)
    {
        _identityService = identityService;
        _tokenService = tokenService;
        _tenantContext = tenantContext;
    }

    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantContext.CurrentTenantId;

        if (tenantId == Guid.Empty || _tenantContext.IsAdminContext)
            throw new DomainException("Registration requires a resolved tenant context.");

        var isFirst = await _identityService.IsFirstUserInTenantAsync(tenantId);
        var role = isFirst ? "Owner" : "User";

        var (success, userId, error) = await _identityService.CreateUserAsync(
            request.Email, request.Password,
            request.FirstName, request.LastName,
            tenantId, role);

        if (!success || userId is null)
            throw new DomainException(error ?? "Registration failed.");

        var accessToken = _tokenService.GenerateAccessToken(userId, request.Email, tenantId, role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _identityService.StoreRefreshTokenAsync(userId, refreshToken);

        return new AuthResult(accessToken, refreshToken, userId, request.Email, role, tenantId, request.FirstName, request.LastName);
    }
}
