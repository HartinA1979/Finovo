using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.UpdateInvoiceDraft;

public record UpdateInvoiceDraftCommand(
    Guid InvoiceId,
    DateTime IssueDate,
    DateTime DueDate,
    string? Notes) : IRequest;
