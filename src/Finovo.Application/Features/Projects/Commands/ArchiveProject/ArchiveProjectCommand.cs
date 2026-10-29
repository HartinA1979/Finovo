using MediatR;

namespace Finovo.Application.Features.Projects.Commands.ArchiveProject;

public record ArchiveProjectCommand(Guid Id) : IRequest;
