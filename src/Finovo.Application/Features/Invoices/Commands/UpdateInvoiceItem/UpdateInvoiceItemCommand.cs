using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.UpdateInvoiceItem;

public record UpdateInvoiceItemCommand(
    Guid InvoiceId,
    Guid ItemId,
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent) : IRequest;
