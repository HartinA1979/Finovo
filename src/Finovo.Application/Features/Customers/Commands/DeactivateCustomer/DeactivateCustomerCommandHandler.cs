using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using MediatR;

namespace Finovo.Application.Features.Customers.Commands.DeactivateCustomer;

public sealed class DeactivateCustomerCommandHandler : IRequestHandler<DeactivateCustomerCommand>
{
    private readonly ICustomerRepository _repository;

    public DeactivateCustomerCommandHandler(ICustomerRepository repository)
        => _repository = repository;

    public async Task Handle(DeactivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Customers.Customer), request.Id);

        customer.Deactivate();

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
