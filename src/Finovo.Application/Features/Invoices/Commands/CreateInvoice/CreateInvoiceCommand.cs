using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.CreateInvoice;

public record CreateInvoiceCommand(
    Guid ProjectId,
    DateTime IssueDate,
    DateTime DueDate,
    string? Notes) : IRequest<Guid>;
