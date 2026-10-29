using Finovo.Domain.Enums;

namespace Finovo.Application.Features.Invoices.DTOs;

public record InvoiceSummaryDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    Guid ProjectId,
    string ProjectName,
    string InvoiceNumber,
    DateTime IssueDate,
    DateTime DueDate,
    InvoiceStatus Status,
    decimal TotalNet,
    decimal TotalGross,
    string Currency,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record InvoiceDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    Guid ProjectId,
    string ProjectName,
    string InvoiceNumber,
    DateTime IssueDate,
    DateTime DueDate,
    InvoiceStatus Status,
    string? Notes,
    IReadOnlyList<InvoiceItemDto> Items,
    decimal TotalNet,
    decimal TaxAmount,
    decimal TotalGross,
    string Currency,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record InvoiceItemDto(
    Guid Id,
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent,
    decimal TotalNet,
    decimal TaxAmount,
    decimal TotalGross);
