using MediatR;

namespace Finovo.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(
    Guid Id,
    string Name,
    string? Description) : IRequest;
