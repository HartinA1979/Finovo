using AutoMapper;
using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using Finovo.Application.Features.Customers.DTOs;
using MediatR;

namespace Finovo.Application.Features.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public GetCustomerByIdQueryHandler(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Customers.Customer), request.Id);

        return _mapper.Map<CustomerDto>(customer);
    }
}
