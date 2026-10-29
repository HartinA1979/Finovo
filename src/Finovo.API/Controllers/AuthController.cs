using Finovo.Application.Features.Auth.Commands.Login;
using Finovo.Application.Features.Auth.Commands.Logout;
using Finovo.Application.Features.Auth.Commands.RefreshToken;
using Finovo.Application.Features.Auth.Commands.Register;
using Finovo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finovo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private const string RefreshTokenCookie = "refresh_token";

    private readonly ISender _sender;

    public AuthController(ISender sender) => _sender = sender;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
    {
        try
        {
            var result = await _sender.Send(command, ct);
            SetRefreshTokenCookie(result.RefreshToken);
            return Ok(new
            {
                result.AccessToken,
                result.UserId,
                result.Email,
                result.Role,
                result.TenantId,
                result.FirstName,
                result.LastName,
            });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        try
        {
            var result = await _sender.Send(command, ct);
            SetRefreshTokenCookie(result.RefreshToken);
            return Ok(new
            {
                result.AccessToken,
                result.UserId,
                result.Email,
                result.Role,
                result.TenantId,
                result.FirstName,
                result.LastName,
            });
        }
        catch (DomainException ex)
        {
            return Unauthorized(new { ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        var token = Request.Cookies[RefreshTokenCookie];
        if (string.IsNullOrEmpty(token))
            return Unauthorized(new { Message = "Refresh token missing." });

        try
        {
            var result = await _sender.Send(new RefreshTokenCommand(token), ct);
            SetRefreshTokenCookie(result.RefreshToken);
            return Ok(new { result.AccessToken });
        }
        catch (DomainException ex)
        {
            return Unauthorized(new { ex.Message });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var token = Request.Cookies[RefreshTokenCookie];
        if (!string.IsNullOrEmpty(token))
            await _sender.Send(new LogoutCommand(token), ct);

        Response.Cookies.Delete(RefreshTokenCookie);
        return NoContent();
    }

    private void SetRefreshTokenCookie(string token)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
        };
        Response.Cookies.Append(RefreshTokenCookie, token, options);
    }
}
