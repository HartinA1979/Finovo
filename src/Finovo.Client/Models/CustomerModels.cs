namespace Finovo.Client.Models;

public record CustomerModel(
    Guid Id,
    string Name,
    string? CompanyName,
    string? Email,
    string? Phone,
    AddressModel? Address,
    string? VatNumber,
    string? Notes,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record AddressModel(
    string Street,
    string City,
    string ZipCode,
    string Country);

public record CreateCustomerRequest(
    string Name,
    string? CompanyName,
    string? Email,
    string? Phone,
    AddressModel? Address,
    string? VatNumber,
    string? Notes);

public record UpdateCustomerRequest(
    string Name,
    string? CompanyName,
    string? Email,
    string? Phone,
    AddressModel? Address,
    string? VatNumber,
    string? Notes);
