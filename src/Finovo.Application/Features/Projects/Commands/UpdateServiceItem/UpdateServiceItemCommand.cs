using MediatR;

namespace Finovo.Application.Features.Projects.Commands.UpdateServiceItem;

public record UpdateServiceItemCommand(
    Guid ProjectId,
    Guid ServiceItemId,
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent) : IRequest;
