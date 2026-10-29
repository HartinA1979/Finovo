using Finovo.Application.Features.Customers.DTOs;
using MediatR;

namespace Finovo.Application.Features.Customers.Queries.GetCustomers;

public record GetCustomersQuery(
    string? NameFilter,
    bool? IsActive) : IRequest<IReadOnlyList<CustomerDto>>;
