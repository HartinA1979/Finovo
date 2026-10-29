using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Entities.Projects;
using MediatR;

namespace Finovo.Application.Features.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _repository;
    private readonly ITenantContext _tenantContext;

    public CreateProjectCommandHandler(IProjectRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project(
            _tenantContext.CurrentTenantId,
            request.CustomerId,
            request.Name,
            request.Description);

        await _repository.AddAsync(project, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}
