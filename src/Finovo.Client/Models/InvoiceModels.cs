namespace Finovo.Client.Models;

public record InvoiceSummaryModel(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    Guid ProjectId,
    string ProjectName,
    string InvoiceNumber,
    DateTime IssueDate,
    DateTime DueDate,
    string Status,
    decimal TotalNet,
    decimal TotalGross,
    string Currency,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record InvoiceModel(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    Guid ProjectId,
    string ProjectName,
    string InvoiceNumber,
    DateTime IssueDate,
    DateTime DueDate,
    string Status,
    string? Notes,
    List<InvoiceItemModel> Items,
    decimal TotalNet,
    decimal TaxAmount,
    decimal TotalGross,
    string Currency,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record InvoiceItemModel(
    Guid Id,
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent,
    decimal TotalNet,
    decimal TaxAmount,
    decimal TotalGross);

public record CreateInvoiceRequest(
    Guid ProjectId,
    DateTime IssueDate,
    DateTime DueDate,
    string? Notes);

public record UpdateInvoiceDraftRequest(
    DateTime IssueDate,
    DateTime DueDate,
    string? Notes);

public record InvoiceItemRequest(
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent);
