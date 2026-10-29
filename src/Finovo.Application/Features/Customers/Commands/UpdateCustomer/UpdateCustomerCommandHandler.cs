using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using Finovo.Domain.ValueObjects;
using MediatR;

namespace Finovo.Application.Features.Customers.Commands.UpdateCustomer;

public sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly ICustomerRepository _repository;

    public UpdateCustomerCommandHandler(ICustomerRepository repository)
        => _repository = repository;

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Customers.Customer), request.Id);

        var email = request.Email is not null ? new Email(request.Email) : null;
        var address = request.Address is not null
            ? new Address(request.Address.Street, request.Address.City, request.Address.ZipCode, request.Address.Country)
            : null;

        customer.Update(request.Name, request.CompanyName, email, request.Phone, address, request.VatNumber, request.Notes);

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
