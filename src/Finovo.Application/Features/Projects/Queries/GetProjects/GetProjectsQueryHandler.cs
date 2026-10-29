using AutoMapper;
using Finovo.Application.Common.Interfaces;
using Finovo.Application.Features.Projects.DTOs;
using MediatR;

namespace Finovo.Application.Features.Projects.Queries.GetProjects;

public sealed class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IReadOnlyList<ProjectSummaryDto>>
{
    private readonly IProjectRepository _projects;
    private readonly ICustomerRepository _customers;
    private readonly IMapper _mapper;

    public GetProjectsQueryHandler(
        IProjectRepository projects,
        ICustomerRepository customers,
        IMapper mapper)
    {
        _projects = projects;
        _customers = customers;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProjectSummaryDto>> Handle(
        GetProjectsQuery request,
        CancellationToken cancellationToken)
    {
        var projects = await _projects.GetAllAsync(request.CustomerId, request.Status, cancellationToken);

        var customerIds = projects.Select(p => p.CustomerId).Distinct();
        var customerNames = await _customers.GetNamesAsync(customerIds, cancellationToken);

        return projects
            .Select(p =>
            {
                var dto = _mapper.Map<ProjectSummaryDto>(p);
                customerNames.TryGetValue(p.CustomerId, out var name);
                return dto with { CustomerName = name ?? "–" };
            })
            .ToList();
    }
}
