using Finovo.Application.Features.Projects.DTOs;
using MediatR;

namespace Finovo.Application.Features.Projects.Queries.GetProjectById;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDto>;
