using Finovo.Application.Features.Projects.DTOs;
using Finovo.Domain.Enums;
using MediatR;

namespace Finovo.Application.Features.Projects.Queries.GetProjects;

public record GetProjectsQuery(
    Guid? CustomerId,
    ProjectStatus? Status) : IRequest<IReadOnlyList<ProjectSummaryDto>>;
