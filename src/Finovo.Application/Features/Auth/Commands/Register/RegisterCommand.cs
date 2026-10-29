using Finovo.Application.Features.Auth.DTOs;
using MediatR;

namespace Finovo.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<AuthResult>;
