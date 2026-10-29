using Finovo.Application.Features.Auth.DTOs;
using MediatR;

namespace Finovo.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<AuthResult>;
