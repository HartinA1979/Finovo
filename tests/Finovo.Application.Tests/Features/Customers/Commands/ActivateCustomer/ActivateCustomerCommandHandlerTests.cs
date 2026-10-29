using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using Finovo.Application.Features.Customers.Commands.ActivateCustomer;
using Finovo.Domain.Entities.Customers;
using FluentAssertions;
using NSubstitute;

namespace Finovo.Application.Tests.Features.Customers.Commands.ActivateCustomer;

public class ActivateCustomerCommandHandlerTests
{
    private readonly ICustomerRepository _repository = Substitute.For<ICustomerRepository>();
    private readonly ActivateCustomerCommandHandler _handler;

    public ActivateCustomerCommandHandlerTests()
        => _handler = new ActivateCustomerCommandHandler(_repository);

    [Fact]
    public async Task Handle_DeactivatedCustomer_ActivatesCustomerAndSavesChanges()
    {
        var customer = new Customer(Guid.NewGuid(), "Test Customer");
        customer.Deactivate();
        _repository.GetByIdAsync(customer.Id, Arg.Any<CancellationToken>()).Returns(customer);

        await _handler.Handle(new ActivateCustomerCommand(customer.Id), CancellationToken.None);

        customer.IsActive.Should().BeTrue();
        await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_CustomerNotFound_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Customer?)null);

        var act = () => _handler.Handle(new ActivateCustomerCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
