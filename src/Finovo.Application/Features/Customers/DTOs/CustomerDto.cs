namespace Finovo.Application.Features.Customers.DTOs;

public record CustomerDto(
    Guid Id,
    string Name,
    string? CompanyName,
    string? Email,
    string? Phone,
    AddressDto? Address,
    string? VatNumber,
    string? Notes,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record AddressDto(
    string Street,
    string City,
    string ZipCode,
    string Country);

public record AddressRequest(
    string Street,
    string City,
    string ZipCode,
    string Country);
