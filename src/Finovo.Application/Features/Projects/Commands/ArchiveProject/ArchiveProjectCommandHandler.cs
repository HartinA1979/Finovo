using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Projects.Commands.ArchiveProject;

public sealed class ArchiveProjectCommandHandler : IRequestHandler<ArchiveProjectCommand>
{
    private readonly IProjectRepository _repository;

    public ArchiveProjectCommandHandler(IProjectRepository repository)
        => _repository = repository;

    public async Task Handle(ArchiveProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Projects.Project), request.Id);

        project.Archive();
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
