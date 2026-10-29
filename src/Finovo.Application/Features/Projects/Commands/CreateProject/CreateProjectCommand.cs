using MediatR;

namespace Finovo.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand(
    Guid CustomerId,
    string Name,
    string? Description) : IRequest<Guid>;
