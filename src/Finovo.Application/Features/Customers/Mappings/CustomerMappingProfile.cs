using AutoMapper;
using Finovo.Application.Features.Customers.DTOs;
using Finovo.Domain.Entities.Customers;

namespace Finovo.Application.Features.Customers.Mappings;

public sealed class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email != null ? s.Email.Value : null))
            .ForMember(d => d.Address, o => o.MapFrom(s =>
                s.Address != null
                    ? new AddressDto(s.Address.Street, s.Address.City, s.Address.ZipCode, s.Address.Country)
                    : null));
    }
}
