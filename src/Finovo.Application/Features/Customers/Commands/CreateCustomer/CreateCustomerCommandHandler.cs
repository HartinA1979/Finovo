using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Entities.Customers;
using Finovo.Domain.ValueObjects;
using MediatR;

namespace Finovo.Application.Features.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _repository;
    private readonly ITenantContext _tenantContext;

    public CreateCustomerCommandHandler(ICustomerRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email is not null ? new Email(request.Email) : null;
        var address = request.Address is not null
            ? new Address(request.Address.Street, request.Address.City, request.Address.ZipCode, request.Address.Country)
            : null;

        var customer = new Customer(
            _tenantContext.CurrentTenantId,
            request.Name,
            request.CompanyName,
            email,
            request.Phone,
            address,
            request.VatNumber);

        if (request.Notes is not null)
            customer.Update(request.Name, request.CompanyName, email, request.Phone, address, request.VatNumber, request.Notes);

        await _repository.AddAsync(customer, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
