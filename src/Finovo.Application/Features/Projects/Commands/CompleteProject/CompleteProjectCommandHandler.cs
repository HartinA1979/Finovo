using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Projects.Commands.CompleteProject;

public sealed class CompleteProjectCommandHandler : IRequestHandler<CompleteProjectCommand>
{
    private readonly IProjectRepository _repository;

    public CompleteProjectCommandHandler(IProjectRepository repository)
        => _repository = repository;

    public async Task Handle(CompleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Projects.Project), request.Id);

        project.Complete();
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
