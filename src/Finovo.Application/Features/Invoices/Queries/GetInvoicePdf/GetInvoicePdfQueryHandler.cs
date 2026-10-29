using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Invoices.Queries.GetInvoicePdf;

public sealed class GetInvoicePdfQueryHandler : IRequestHandler<GetInvoicePdfQuery, byte[]>
{
    private readonly IInvoiceRepository _invoices;
    private readonly ICustomerRepository _customers;
    private readonly ITenantRepository _tenants;
    private readonly ITenantContext _tenantContext;
    private readonly IPdfService _pdf;

    public GetInvoicePdfQueryHandler(
        IInvoiceRepository invoices,
        ICustomerRepository customers,
        ITenantRepository tenants,
        ITenantContext tenantContext,
        IPdfService pdf)
    {
        _invoices = invoices;
        _customers = customers;
        _tenants = tenants;
        _tenantContext = tenantContext;
        _pdf = pdf;
    }

    public async Task<byte[]> Handle(GetInvoicePdfQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice", request.InvoiceId);

        var customer = await _customers.GetByIdAsync(invoice.CustomerId, cancellationToken)
            ?? throw new NotFoundException("Customer", invoice.CustomerId);

        var tenant = await _tenants.GetByIdAsync(_tenantContext.CurrentTenantId, cancellationToken)
            ?? throw new NotFoundException("Tenant", _tenantContext.CurrentTenantId);

        var items = invoice.Items.Select(i => new InvoicePdfItem(
            i.Description,
            i.Hours,
            i.HourlyRate.Amount,
            (int)i.TaxRate,
            i.TotalNet.Amount,
            i.TaxAmount.Amount,
            i.TotalGross.Amount)).ToList();

        var data = new InvoicePdfData(
            InvoiceNumber: invoice.InvoiceNumber.ToString(),
            IssueDate: invoice.IssueDate,
            DueDate: invoice.DueDate,
            SellerName: tenant.Name,
            SellerCompany: tenant.CompanyName,
            SellerEmail: tenant.Email?.Value,
            SellerPhone: tenant.Phone,
            SellerAddress: tenant.Address?.ToString(),
            SellerVatNumber: tenant.VatNumber,
            BuyerName: customer.Name,
            BuyerCompany: customer.CompanyName,
            BuyerEmail: customer.Email?.Value,
            BuyerAddress: customer.Address?.ToString(),
            BuyerVatNumber: customer.VatNumber,
            Notes: invoice.Notes,
            Items: items,
            TotalNet: invoice.TotalNet.Amount,
            TaxAmount: invoice.TaxAmount.Amount,
            TotalGross: invoice.TotalGross.Amount,
            Currency: invoice.TotalGross.Currency);

        return _pdf.GenerateInvoicePdf(data);
    }
}
