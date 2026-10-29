using Finovo.Application.Features.Invoices.DTOs;
using MediatR;

namespace Finovo.Application.Features.Invoices.Queries.GetInvoiceById;

public record GetInvoiceByIdQuery(Guid InvoiceId) : IRequest<InvoiceDto>;
