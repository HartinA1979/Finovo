using MediatR;

namespace Finovo.Application.Features.Customers.Commands.ActivateCustomer;

public record ActivateCustomerCommand(Guid Id) : IRequest;
