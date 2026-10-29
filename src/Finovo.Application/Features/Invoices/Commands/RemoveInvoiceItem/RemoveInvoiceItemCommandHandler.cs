using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.RemoveInvoiceItem;

public sealed class RemoveInvoiceItemCommandHandler : IRequestHandler<RemoveInvoiceItemCommand>
{
    private readonly IInvoiceRepository _invoices;

    public RemoveInvoiceItemCommandHandler(IInvoiceRepository invoices) => _invoices = invoices;

    public async Task Handle(RemoveInvoiceItemCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice", request.InvoiceId);

        invoice.RemoveItem(request.ItemId);
        await _invoices.SaveChangesAsync(cancellationToken);
    }
}
