using Finovo.Application.Features.Customers.DTOs;
using MediatR;

namespace Finovo.Application.Features.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string Name,
    string? CompanyName,
    string? Email,
    string? Phone,
    AddressRequest? Address,
    string? VatNumber,
    string? Notes) : IRequest<Guid>;
