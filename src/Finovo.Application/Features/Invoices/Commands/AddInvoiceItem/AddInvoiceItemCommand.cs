using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.AddInvoiceItem;

public record AddInvoiceItemCommand(
    Guid InvoiceId,
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent) : IRequest<Guid>;
