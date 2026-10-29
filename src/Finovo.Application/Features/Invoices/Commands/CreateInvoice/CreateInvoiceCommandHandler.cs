using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Entities.Invoices;
using Finovo.Domain.Exceptions;
using Finovo.Domain.ValueObjects;
using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.CreateInvoice;

public sealed class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Guid>
{
    private readonly IInvoiceRepository _invoices;
    private readonly IProjectRepository _projects;
    private readonly ITenantContext _tenant;

    public CreateInvoiceCommandHandler(
        IInvoiceRepository invoices,
        IProjectRepository projects,
        ITenantContext tenant)
    {
        _invoices = invoices;
        _projects = projects;
        _tenant = tenant;
    }

    public async Task<Guid> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var project = await _projects.GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Project", request.ProjectId);

        if (project.ServiceItems.Count == 0)
            throw new DomainException("Projekt hat keine Leistungen – Rechnung kann nicht erstellt werden.");

        var tenantId = _tenant.CurrentTenantId;
        var year = request.IssueDate.Year;
        var seq = await _invoices.GetNextSequenceNumberAsync(tenantId, year, cancellationToken);

        var invoice = new Invoice(
            tenantId,
            project.Id,
            project.CustomerId,
            new InvoiceNumber(year, seq),
            request.IssueDate,
            request.DueDate,
            request.Notes);

        foreach (var si in project.ServiceItems)
            invoice.AddItem(si.Description, si.Hours, si.HourlyRate, si.TaxRate);

        project.MarkAsInvoiced();

        await _invoices.AddAsync(invoice, cancellationToken);
        await _invoices.SaveChangesAsync(cancellationToken);

        return invoice.Id;
    }
}
