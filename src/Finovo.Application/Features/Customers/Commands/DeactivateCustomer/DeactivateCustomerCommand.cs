using MediatR;

namespace Finovo.Application.Features.Customers.Commands.DeactivateCustomer;

public record DeactivateCustomerCommand(Guid Id) : IRequest;
