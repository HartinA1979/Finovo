using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Enums;
using Finovo.Domain.ValueObjects;
using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.AddInvoiceItem;

public sealed class AddInvoiceItemCommandHandler : IRequestHandler<AddInvoiceItemCommand, Guid>
{
    private readonly IInvoiceRepository _invoices;

    public AddInvoiceItemCommandHandler(IInvoiceRepository invoices) => _invoices = invoices;

    public async Task<Guid> Handle(AddInvoiceItemCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice", request.InvoiceId);

        var taxRate = (TaxRate)request.TaxRatePercent;
        var hourlyRate = new Money(request.HourlyRateAmount, request.Currency);
        var item = invoice.AddItem(request.Description, request.Hours, hourlyRate, taxRate);

        await _invoices.SaveChangesAsync(cancellationToken);
        return item.Id;
    }
}
