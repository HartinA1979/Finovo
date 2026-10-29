using AutoMapper;
using Finovo.Application.Common.Interfaces;
using Finovo.Application.Features.Invoices.DTOs;
using MediatR;

namespace Finovo.Application.Features.Invoices.Queries.GetInvoices;

public sealed class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, IReadOnlyList<InvoiceSummaryDto>>
{
    private readonly IInvoiceRepository _invoices;
    private readonly ICustomerRepository _customers;
    private readonly IProjectRepository _projects;
    private readonly IMapper _mapper;

    public GetInvoicesQueryHandler(
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

    public async Task<IReadOnlyList<InvoiceSummaryDto>> Handle(
        GetInvoicesQuery request,
        CancellationToken cancellationToken)
    {
        var invoices = await _invoices.GetAllAsync(request.CustomerId, request.Status, cancellationToken);

        var customerIds = invoices.Select(i => i.CustomerId).Distinct();
        var projectIds = invoices.Select(i => i.ProjectId).Distinct();

        var customerNames = await _customers.GetNamesAsync(customerIds, cancellationToken);
        var projectNames = await _projects.GetNamesAsync(projectIds, cancellationToken);

        return invoices
            .Select(i =>
            {
                var dto = _mapper.Map<InvoiceSummaryDto>(i);
                customerNames.TryGetValue(i.CustomerId, out var cName);
                projectNames.TryGetValue(i.ProjectId, out var pName);
                return dto with { CustomerName = cName ?? "–", ProjectName = pName ?? "–" };
            })
            .ToList();
    }
}
