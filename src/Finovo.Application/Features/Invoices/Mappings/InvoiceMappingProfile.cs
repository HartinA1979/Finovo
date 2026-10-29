using AutoMapper;
using Finovo.Application.Features.Invoices.DTOs;
using Finovo.Domain.Entities.Invoices;

namespace Finovo.Application.Features.Invoices.Mappings;

public sealed class InvoiceMappingProfile : Profile
{
    public InvoiceMappingProfile()
    {
        CreateMap<InvoiceItem, InvoiceItemDto>()
            .ForMember(d => d.HourlyRateAmount, o => o.MapFrom(s => s.HourlyRate.Amount))
            .ForMember(d => d.Currency, o => o.MapFrom(s => s.HourlyRate.Currency))
            .ForMember(d => d.TaxRatePercent, o => o.MapFrom(s => (int)s.TaxRate))
            .ForMember(d => d.TotalNet, o => o.MapFrom(s => s.TotalNet.Amount))
            .ForMember(d => d.TaxAmount, o => o.MapFrom(s => s.TaxAmount.Amount))
            .ForMember(d => d.TotalGross, o => o.MapFrom(s => s.TotalGross.Amount));

        // CustomerName and ProjectName are enriched manually via `with`
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(d => d.CustomerName, o => o.Ignore())
            .ForMember(d => d.ProjectName, o => o.Ignore())
            .ForMember(d => d.InvoiceNumber, o => o.MapFrom(s => s.InvoiceNumber.ToString()))
            .ForMember(d => d.TotalNet, o => o.MapFrom(s => s.TotalNet.Amount))
            .ForMember(d => d.TaxAmount, o => o.MapFrom(s => s.TaxAmount.Amount))
            .ForMember(d => d.TotalGross, o => o.MapFrom(s => s.TotalGross.Amount))
            .ForMember(d => d.Currency, o => o.MapFrom(s => s.TotalNet.Currency));

        CreateMap<Invoice, InvoiceSummaryDto>()
            .ForMember(d => d.CustomerName, o => o.Ignore())
            .ForMember(d => d.ProjectName, o => o.Ignore())
            .ForMember(d => d.InvoiceNumber, o => o.MapFrom(s => s.InvoiceNumber.ToString()))
            .ForMember(d => d.TotalNet, o => o.MapFrom(s => s.TotalNet.Amount))
            .ForMember(d => d.TotalGross, o => o.MapFrom(s => s.TotalGross.Amount))
            .ForMember(d => d.Currency, o => o.MapFrom(s => s.TotalNet.Currency));
    }
}
