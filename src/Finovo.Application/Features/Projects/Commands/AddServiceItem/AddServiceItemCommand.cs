using MediatR;

namespace Finovo.Application.Features.Projects.Commands.AddServiceItem;

public record AddServiceItemCommand(
    Guid ProjectId,
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent) : IRequest<Guid>;
