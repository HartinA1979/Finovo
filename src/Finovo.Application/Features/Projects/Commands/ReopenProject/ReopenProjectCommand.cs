using MediatR;

namespace Finovo.Application.Features.Projects.Commands.ReopenProject;

public record ReopenProjectCommand(Guid Id) : IRequest;
