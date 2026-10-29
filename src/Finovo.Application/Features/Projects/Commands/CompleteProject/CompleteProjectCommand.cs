using MediatR;

namespace Finovo.Application.Features.Projects.Commands.CompleteProject;

public record CompleteProjectCommand(Guid Id) : IRequest;
