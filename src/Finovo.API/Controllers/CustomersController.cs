using Finovo.Application.Features.Customers.Commands.CreateCustomer;
using Finovo.Application.Features.Customers.Commands.DeactivateCustomer;
using Finovo.Application.Features.Customers.Commands.UpdateCustomer;
using Finovo.Application.Features.Customers.DTOs;
using Finovo.Application.Features.Customers.Queries.GetCustomerById;
using Finovo.Application.Features.Customers.Queries.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finovo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CustomersController : ControllerBase
{
    private readonly ISender _sender;

    public CustomersController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetAll(
        [FromQuery] string? nameFilter,
        [FromQuery] bool? isActive,
        CancellationToken ct)
    {
        var result = await _sender.Send(new GetCustomersQuery(nameFilter, isActive), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetCustomerByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCustomerCommand command, CancellationToken ct)
    {
        var id = await _sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken ct)
    {
        var command = new UpdateCustomerCommand(
            id,
            request.Name,
            request.CompanyName,
            request.Email,
            request.Phone,
            request.Address,
            request.VatNumber,
            request.Notes);

        await _sender.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeactivateCustomerCommand(id), ct);
        return NoContent();
    }
}

// Separate request body type for PUT to avoid route-param binding conflict with record command
public record UpdateCustomerRequest(
    string Name,
    string? CompanyName,
    string? Email,
    string? Phone,
    AddressRequest? Address,
    string? VatNumber,
    string? Notes);
