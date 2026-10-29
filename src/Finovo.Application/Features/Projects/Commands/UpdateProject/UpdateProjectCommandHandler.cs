using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Projects.Commands.UpdateProject;

public sealed class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository _repository;

    public UpdateProjectCommandHandler(IProjectRepository repository)
        => _repository = repository;

    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Projects.Project), request.Id);

        project.Update(request.Name, request.Description);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
