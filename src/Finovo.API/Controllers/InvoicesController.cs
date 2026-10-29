using Finovo.Application.Features.Invoices.Commands.AddInvoiceItem;
using Finovo.Application.Features.Invoices.Commands.CancelInvoice;
using Finovo.Application.Features.Invoices.Commands.CreateInvoice;
using Finovo.Application.Features.Invoices.Commands.MarkInvoicePaid;
using Finovo.Application.Features.Invoices.Commands.RemoveInvoiceItem;
using Finovo.Application.Features.Invoices.Commands.SendInvoice;
using Finovo.Application.Features.Invoices.Commands.UpdateInvoiceDraft;
using Finovo.Application.Features.Invoices.Commands.UpdateInvoiceItem;
using Finovo.Application.Features.Invoices.DTOs;
using Finovo.Application.Features.Invoices.Queries.GetInvoiceById;
using Finovo.Application.Features.Invoices.Queries.GetInvoicePdf;
using Finovo.Application.Features.Invoices.Queries.GetInvoices;
using Finovo.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finovo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class InvoicesController : ControllerBase
{
    private readonly ISender _sender;

    public InvoicesController(ISender sender) => _sender = sender;

    // ── Invoices ──────────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<InvoiceSummaryDto>>> GetAll(
        [FromQuery] Guid? customerId,
        [FromQuery] InvoiceStatus? status,
        CancellationToken ct)
    {
        var result = await _sender.Send(new GetInvoicesQuery(customerId, status), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InvoiceDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetInvoiceByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateInvoiceRequest request,
        CancellationToken ct)
    {
        var id = await _sender.Send(new CreateInvoiceCommand(
            request.ProjectId,
            request.IssueDate,
            request.DueDate,
            request.Notes), ct);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateDraft(
        Guid id,
        [FromBody] UpdateInvoiceDraftRequest request,
        CancellationToken ct)
    {
        await _sender.Send(new UpdateInvoiceDraftCommand(id, request.IssueDate, request.DueDate, request.Notes), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/send")]
    public async Task<IActionResult> Send(Guid id, CancellationToken ct)
    {
        await _sender.Send(new SendInvoiceCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/pay")]
    public async Task<IActionResult> Pay(Guid id, CancellationToken ct)
    {
        await _sender.Send(new MarkInvoicePaidCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        await _sender.Send(new CancelInvoiceCommand(id), ct);
        return NoContent();
    }

    [HttpGet("{id:guid}/pdf")]
    public async Task<IActionResult> GetPdf(Guid id, CancellationToken ct)
    {
        var bytes = await _sender.Send(new GetInvoicePdfQuery(id), ct);
        return File(bytes, "application/pdf", $"Rechnung.pdf");
    }

    // ── Invoice Items ─────────────────────────────────────────────────────────

    [HttpPost("{id:guid}/items")]
    public async Task<ActionResult<Guid>> AddItem(
        Guid id,
        [FromBody] InvoiceItemRequest request,
        CancellationToken ct)
    {
        var itemId = await _sender.Send(new AddInvoiceItemCommand(
            id,
            request.Description,
            request.Hours,
            request.HourlyRateAmount,
            request.Currency,
            request.TaxRatePercent), ct);

        return Ok(itemId);
    }

    [HttpPut("{id:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> UpdateItem(
        Guid id,
        Guid itemId,
        [FromBody] InvoiceItemRequest request,
        CancellationToken ct)
    {
        await _sender.Send(new UpdateInvoiceItemCommand(
            id,
            itemId,
            request.Description,
            request.Hours,
            request.HourlyRateAmount,
            request.Currency,
            request.TaxRatePercent), ct);

        return NoContent();
    }

    [HttpDelete("{id:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid id, Guid itemId, CancellationToken ct)
    {
        await _sender.Send(new RemoveInvoiceItemCommand(id, itemId), ct);
        return NoContent();
    }
}

// ── Request body types ────────────────────────────────────────────────────────

public record CreateInvoiceRequest(
    Guid ProjectId,
    DateTime IssueDate,
    DateTime DueDate,
    string? Notes);

public record UpdateInvoiceDraftRequest(
    DateTime IssueDate,
    DateTime DueDate,
    string? Notes);

public record InvoiceItemRequest(
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent);
