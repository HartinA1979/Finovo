using AutoMapper;
using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using Finovo.Application.Features.Projects.DTOs;
using MediatR;

namespace Finovo.Application.Features.Projects.Queries.GetProjectById;

public sealed class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    private readonly IProjectRepository _projects;
    private readonly ICustomerRepository _customers;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(
        IProjectRepository projects,
        ICustomerRepository customers,
        IMapper mapper)
    {
        _projects = projects;
        _customers = customers;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projects.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Projects.Project), request.Id);

        var customer = await _customers.GetByIdAsync(project.CustomerId, cancellationToken);
        var dto = _mapper.Map<ProjectDto>(project);
        return dto with { CustomerName = customer?.Name ?? "–" };
    }
}
