using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Projects.Commands.ReopenProject;

public sealed class ReopenProjectCommandHandler : IRequestHandler<ReopenProjectCommand>
{
    private readonly IProjectRepository _repository;

    public ReopenProjectCommandHandler(IProjectRepository repository)
        => _repository = repository;

    public async Task Handle(ReopenProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Projects.Project), request.Id);

        project.Reopen();
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
