using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Projects.Commands.RemoveServiceItem;

public sealed class RemoveServiceItemCommandHandler : IRequestHandler<RemoveServiceItemCommand>
{
    private readonly IProjectRepository _repository;

    public RemoveServiceItemCommandHandler(IProjectRepository repository)
        => _repository = repository;

    public async Task Handle(RemoveServiceItemCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Projects.Project), request.ProjectId);

        project.RemoveServiceItem(request.ServiceItemId);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
