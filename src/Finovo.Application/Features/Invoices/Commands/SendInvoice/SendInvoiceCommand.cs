using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.SendInvoice;

public record SendInvoiceCommand(Guid InvoiceId) : IRequest;
