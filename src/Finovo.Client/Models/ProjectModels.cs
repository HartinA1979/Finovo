namespace Finovo.Client.Models;

public record ProjectSummaryModel(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    string Name,
    string? Description,
    string Status,
    decimal TotalNet,
    decimal TotalGross,
    string Currency,
    int ServiceItemCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record ProjectModel(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    string Name,
    string? Description,
    string Status,
    List<ServiceItemModel> ServiceItems,
    decimal TotalNet,
    decimal TotalGross,
    string Currency,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record ServiceItemModel(
    Guid Id,
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent,
    decimal TotalNet,
    decimal TaxAmount,
    decimal TotalGross);

public record CreateProjectRequest(
    Guid CustomerId,
    string Name,
    string? Description);

public record UpdateProjectRequest(string Name, string? Description);

public record ServiceItemRequest(
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent);
