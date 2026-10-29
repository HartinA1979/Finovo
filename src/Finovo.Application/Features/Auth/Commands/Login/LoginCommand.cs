using Finovo.Application.Features.Auth.DTOs;
using MediatR;

namespace Finovo.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResult>;
