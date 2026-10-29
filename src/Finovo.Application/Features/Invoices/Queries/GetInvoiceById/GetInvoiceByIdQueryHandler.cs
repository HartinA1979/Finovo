using AutoMapper;
using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using Finovo.Application.Features.Invoices.DTOs;
using MediatR;

namespace Finovo.Application.Features.Invoices.Queries.GetInvoiceById;

public sealed class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto>
{
    private readonly IInvoiceRepository _invoices;
    private readonly ICustomerRepository _customers;
    private readonly IProjectRepository _projects;
    private readonly IMapper _mapper;

    public GetInvoiceByIdQueryHandler(
        IInvoiceRepository invoices,
        ICustomerRepository customers,
        IProjectRepository projects,
        IMapper mapper)
    {
        _invoices = invoices;
        _customers = customers;
        _projects = projects;
        _mapper = mapper;
    }

    public async Task<InvoiceDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice", request.InvoiceId);

        var customerNames = await _customers.GetNamesAsync([invoice.CustomerId], cancellationToken);
        var projectNames = await _projects.GetNamesAsync([invoice.ProjectId], cancellationToken);

        customerNames.TryGetValue(invoice.CustomerId, out var cName);
        projectNames.TryGetValue(invoice.ProjectId, out var pName);

        var dto = _mapper.Map<InvoiceDto>(invoice);
        return dto with { CustomerName = cName ?? "–", ProjectName = pName ?? "–" };
    }
}
