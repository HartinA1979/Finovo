using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.SendInvoice;

public sealed class SendInvoiceCommandHandler : IRequestHandler<SendInvoiceCommand>
{
    private readonly IInvoiceRepository _invoices;

    public SendInvoiceCommandHandler(IInvoiceRepository invoices) => _invoices = invoices;

    public async Task Handle(SendInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice", request.InvoiceId);

        invoice.Send();
        await _invoices.SaveChangesAsync(cancellationToken);
    }
}
