using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.MarkInvoicePaid;

public sealed class MarkInvoicePaidCommandHandler : IRequestHandler<MarkInvoicePaidCommand>
{
    private readonly IInvoiceRepository _invoices;

    public MarkInvoicePaidCommandHandler(IInvoiceRepository invoices) => _invoices = invoices;

    public async Task Handle(MarkInvoicePaidCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice", request.InvoiceId);

        invoice.MarkAsPaid();
        await _invoices.SaveChangesAsync(cancellationToken);
    }
}
