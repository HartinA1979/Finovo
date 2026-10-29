using Finovo.Domain.Common;
using Finovo.Domain.Enums;
using Finovo.Domain.Events;
using Finovo.Domain.Exceptions;
using Finovo.Domain.ValueObjects;

namespace Finovo.Domain.Entities.Invoices;

public class Invoice : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid CustomerId { get; private set; }
    public InvoiceNumber InvoiceNumber { get; private set; }
    public DateTime IssueDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public string? Notes { get; private set; }

    private readonly List<InvoiceItem> _items = [];
    public IReadOnlyList<InvoiceItem> Items => _items.AsReadOnly();

    public Money TotalNet => _items.Aggregate(Money.Zero(), (acc, item) => acc.Add(item.TotalNet));
    public Money TotalGross => _items.Aggregate(Money.Zero(), (acc, item) => acc.Add(item.TotalGross));
    public Money TaxAmount => _items.Aggregate(Money.Zero(), (acc, item) => acc.Add(item.TaxAmount));

    private Invoice() { InvoiceNumber = null!; } // EF Core

    public Invoice(
        Guid tenantId,
        Guid projectId,
        Guid customerId,
        InvoiceNumber invoiceNumber,
        DateTime issueDate,
        DateTime dueDate,
        string? notes = null)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId is required.", nameof(tenantId));
        if (projectId == Guid.Empty) throw new ArgumentException("ProjectId is required.", nameof(projectId));
        if (customerId == Guid.Empty) throw new ArgumentException("CustomerId is required.", nameof(customerId));
        if (dueDate.Date < issueDate.Date)
            throw new ArgumentException("Due date must be on or after issue date.", nameof(dueDate));

        TenantId = tenantId;
        ProjectId = projectId;
        CustomerId = customerId;
        InvoiceNumber = invoiceNumber;
        IssueDate = issueDate.Date;
        DueDate = dueDate.Date;
        Notes = notes?.Trim();
        Status = InvoiceStatus.Draft;
    }

    public InvoiceItem AddItem(string description, decimal hours, Money hourlyRate, TaxRate taxRate)
    {
        EnsureDraft();
        var item = new InvoiceItem(Id, description, hours, hourlyRate, taxRate);
        _items.Add(item);
        SetUpdated();
        return item;
    }

    public void UpdateItem(Guid itemId, string description, decimal hours, Money hourlyRate, TaxRate taxRate)
    {
        EnsureDraft();
        var item = _items.FirstOrDefault(x => x.Id == itemId)
            ?? throw new DomainException("Invoice item not found.");
        item.Update(description, hours, hourlyRate, taxRate);
        SetUpdated();
    }

    public void RemoveItem(Guid itemId)
    {
        EnsureDraft();
        var item = _items.FirstOrDefault(x => x.Id == itemId)
            ?? throw new DomainException("Invoice item not found.");
        _items.Remove(item);
        SetUpdated();
    }

    public void UpdateDraft(DateTime issueDate, DateTime dueDate, string? notes)
    {
        EnsureDraft();
        if (dueDate.Date < issueDate.Date)
            throw new ArgumentException("Due date must be on or after issue date.", nameof(dueDate));

        IssueDate = issueDate.Date;
        DueDate = dueDate.Date;
        Notes = notes?.Trim();
        SetUpdated();
    }

    public void UpdateDueDate(DateTime dueDate)
    {
        if (Status is InvoiceStatus.Paid or InvoiceStatus.Cancelled)
            throw new DomainException("Cannot update a paid or cancelled invoice.");
        if (dueDate.Date < IssueDate)
            throw new ArgumentException("Due date must be on or after issue date.", nameof(dueDate));

        DueDate = dueDate.Date;
        SetUpdated();
    }

    public void Send()
    {
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("Only draft invoices can be sent.");
        if (_items.Count == 0)
            throw new DomainException("Cannot send an invoice without items.");

        Status = InvoiceStatus.Sent;
        SetUpdated();
    }

    public void MarkAsPaid()
    {
        if (Status != InvoiceStatus.Sent)
            throw new DomainException("Only sent invoices can be marked as paid.");

        Status = InvoiceStatus.Paid;
        SetUpdated();
        AddDomainEvent(new InvoicePaidDomainEvent(Id, TenantId, ProjectId, CustomerId, TotalGross));
    }

    public void Cancel()
    {
        if (Status == InvoiceStatus.Paid)
            throw new DomainException("Paid invoices cannot be cancelled.");
        if (Status == InvoiceStatus.Cancelled)
            throw new DomainException("Invoice is already cancelled.");

        Status = InvoiceStatus.Cancelled;
        SetUpdated();
    }

    private void EnsureDraft()
    {
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("Only draft invoices can be modified.");
    }
}
