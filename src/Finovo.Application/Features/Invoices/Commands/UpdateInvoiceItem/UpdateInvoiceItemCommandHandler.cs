using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Enums;
using Finovo.Domain.ValueObjects;
using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.UpdateInvoiceItem;

public sealed class UpdateInvoiceItemCommandHandler : IRequestHandler<UpdateInvoiceItemCommand>
{
    private readonly IInvoiceRepository _invoices;

    public UpdateInvoiceItemCommandHandler(IInvoiceRepository invoices) => _invoices = invoices;

    public async Task Handle(UpdateInvoiceItemCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice", request.InvoiceId);

        var taxRate = (TaxRate)request.TaxRatePercent;
        var hourlyRate = new Money(request.HourlyRateAmount, request.Currency);
        invoice.UpdateItem(request.ItemId, request.Description, request.Hours, hourlyRate, taxRate);

        await _invoices.SaveChangesAsync(cancellationToken);
    }
}
