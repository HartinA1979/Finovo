using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.CancelInvoice;

public sealed class CancelInvoiceCommandHandler : IRequestHandler<CancelInvoiceCommand>
{
    private readonly IInvoiceRepository _invoices;

    public CancelInvoiceCommandHandler(IInvoiceRepository invoices) => _invoices = invoices;

    public async Task Handle(CancelInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice", request.InvoiceId);

        invoice.Cancel();
        await _invoices.SaveChangesAsync(cancellationToken);
    }
}
