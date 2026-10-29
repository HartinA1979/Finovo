using Finovo.Application.Features.Customers.DTOs;
using MediatR;

namespace Finovo.Application.Features.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(
    Guid Id,
    string Name,
    string? CompanyName,
    string? Email,
    string? Phone,
    AddressRequest? Address,
    string? VatNumber,
    string? Notes) : IRequest;
