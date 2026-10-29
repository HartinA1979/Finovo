using Finovo.Domain.Enums;

namespace Finovo.Application.Features.Projects.DTOs;

public record ProjectSummaryDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    string Name,
    string? Description,
    ProjectStatus Status,
    decimal TotalNet,
    decimal TotalGross,
    string Currency,
    int ServiceItemCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record ProjectDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    string Name,
    string? Description,
    ProjectStatus Status,
    IReadOnlyList<ServiceItemDto> ServiceItems,
    decimal TotalNet,
    decimal TotalGross,
    string Currency,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record ServiceItemDto(
    Guid Id,
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent,
    decimal TotalNet,
    decimal TaxAmount,
    decimal TotalGross);
