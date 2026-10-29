using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Customers.Commands.ActivateCustomer;

public sealed class ActivateCustomerCommandHandler : IRequestHandler<ActivateCustomerCommand>
{
    private readonly ICustomerRepository _repository;

    public ActivateCustomerCommandHandler(ICustomerRepository repository)
        => _repository = repository;

    public async Task Handle(ActivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Customers.Customer), request.Id);

        customer.Activate();

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
