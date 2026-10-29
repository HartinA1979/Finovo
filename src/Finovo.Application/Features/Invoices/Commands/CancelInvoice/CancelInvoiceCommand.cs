using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.CancelInvoice;

public record CancelInvoiceCommand(Guid InvoiceId) : IRequest;
