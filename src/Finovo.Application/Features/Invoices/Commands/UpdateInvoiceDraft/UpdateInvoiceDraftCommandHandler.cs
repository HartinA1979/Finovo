using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.UpdateInvoiceDraft;

public sealed class UpdateInvoiceDraftCommandHandler : IRequestHandler<UpdateInvoiceDraftCommand>
{
    private readonly IInvoiceRepository _invoices;

    public UpdateInvoiceDraftCommandHandler(IInvoiceRepository invoices) => _invoices = invoices;

    public async Task Handle(UpdateInvoiceDraftCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice", request.InvoiceId);

        invoice.UpdateDraft(request.IssueDate, request.DueDate, request.Notes);
        await _invoices.SaveChangesAsync(cancellationToken);
    }
}
