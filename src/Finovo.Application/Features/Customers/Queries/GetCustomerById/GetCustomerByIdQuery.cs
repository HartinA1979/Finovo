using Finovo.Application.Features.Customers.DTOs;
using MediatR;

namespace Finovo.Application.Features.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto>;
