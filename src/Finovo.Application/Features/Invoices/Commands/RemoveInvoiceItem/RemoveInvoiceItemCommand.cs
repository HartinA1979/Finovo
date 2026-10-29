using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.RemoveInvoiceItem;

public record RemoveInvoiceItemCommand(Guid InvoiceId, Guid ItemId) : IRequest;
