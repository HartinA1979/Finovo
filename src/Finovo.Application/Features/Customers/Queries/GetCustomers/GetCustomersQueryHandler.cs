using AutoMapper;
using Finovo.Application.Common.Interfaces;
using Finovo.Application.Features.Customers.DTOs;
using MediatR;

namespace Finovo.Application.Features.Customers.Queries.GetCustomers;

public sealed class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IReadOnlyList<CustomerDto>>
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _repository.GetAllAsync(request.NameFilter, request.IsActive, cancellationToken);
        return _mapper.Map<IReadOnlyList<CustomerDto>>(customers);
    }
}
