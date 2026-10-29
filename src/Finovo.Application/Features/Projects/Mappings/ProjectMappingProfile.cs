using AutoMapper;
using Finovo.Application.Features.Projects.DTOs;
using Finovo.Domain.Entities.Projects;

namespace Finovo.Application.Features.Projects.Mappings;

public sealed class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ServiceItem, ServiceItemDto>()
            .ForMember(d => d.HourlyRateAmount, o => o.MapFrom(s => s.HourlyRate.Amount))
            .ForMember(d => d.Currency, o => o.MapFrom(s => s.HourlyRate.Currency))
            .ForMember(d => d.TaxRatePercent, o => o.MapFrom(s => (int)s.TaxRate))
            .ForMember(d => d.TotalNet, o => o.MapFrom(s => s.TotalNet.Amount))
            .ForMember(d => d.TaxAmount, o => o.MapFrom(s => s.TaxAmount.Amount))
            .ForMember(d => d.TotalGross, o => o.MapFrom(s => s.TotalGross.Amount));

        // ProjectDto: CustomerName is set manually after mapping via `with`
        CreateMap<Project, ProjectDto>()
            .ForMember(d => d.CustomerName, o => o.Ignore())
            .ForMember(d => d.TotalNet, o => o.MapFrom(s => s.TotalNet.Amount))
            .ForMember(d => d.TotalGross, o => o.MapFrom(s => s.TotalGross.Amount))
            .ForMember(d => d.Currency, o => o.MapFrom(s => s.TotalNet.Currency));

        CreateMap<Project, ProjectSummaryDto>()
            .ForMember(d => d.CustomerName, o => o.Ignore())
            .ForMember(d => d.TotalNet, o => o.MapFrom(s => s.TotalNet.Amount))
            .ForMember(d => d.TotalGross, o => o.MapFrom(s => s.TotalGross.Amount))
            .ForMember(d => d.Currency, o => o.MapFrom(s => s.TotalNet.Currency))
            .ForMember(d => d.ServiceItemCount, o => o.MapFrom(s => s.ServiceItems.Count));
    }
}
