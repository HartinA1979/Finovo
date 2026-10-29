using MediatR;

namespace Finovo.Application.Features.Projects.Commands.RemoveServiceItem;

public record RemoveServiceItemCommand(Guid ProjectId, Guid ServiceItemId) : IRequest;
