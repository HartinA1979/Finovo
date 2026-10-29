using Finovo.Application.Features.Projects.Commands.AddServiceItem;
using Finovo.Application.Features.Projects.Commands.ArchiveProject;
using Finovo.Application.Features.Projects.Commands.CompleteProject;
using Finovo.Application.Features.Projects.Commands.CreateProject;
using Finovo.Application.Features.Projects.Commands.RemoveServiceItem;
using Finovo.Application.Features.Projects.Commands.ReopenProject;
using Finovo.Application.Features.Projects.Commands.UpdateProject;
using Finovo.Application.Features.Projects.Commands.UpdateServiceItem;
using Finovo.Application.Features.Projects.DTOs;
using Finovo.Application.Features.Projects.Queries.GetProjectById;
using Finovo.Application.Features.Projects.Queries.GetProjects;
using Finovo.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finovo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ProjectsController : ControllerBase
{
    private readonly ISender _sender;

    public ProjectsController(ISender sender) => _sender = sender;

    // ── Projects ──────────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProjectSummaryDto>>> GetAll(
        [FromQuery] Guid? customerId,
        [FromQuery] ProjectStatus? status,
        CancellationToken ct)
    {
        var result = await _sender.Send(new GetProjectsQuery(customerId, status), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetProjectByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateProjectCommand command,
        CancellationToken ct)
    {
        var id = await _sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProjectRequest request,
        CancellationToken ct)
    {
        await _sender.Send(new UpdateProjectCommand(id, request.Name, request.Description), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken ct)
    {
        await _sender.Send(new CompleteProjectCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/reopen")]
    public async Task<IActionResult> Reopen(Guid id, CancellationToken ct)
    {
        await _sender.Send(new ReopenProjectCommand(id), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct)
    {
        await _sender.Send(new ArchiveProjectCommand(id), ct);
        return NoContent();
    }

    // ── Service Items ─────────────────────────────────────────────────────────

    [HttpPost("{id:guid}/service-items")]
    public async Task<ActionResult<Guid>> AddServiceItem(
        Guid id,
        [FromBody] ServiceItemRequest request,
        CancellationToken ct)
    {
        var itemId = await _sender.Send(new AddServiceItemCommand(
            id,
            request.Description,
            request.Hours,
            request.HourlyRateAmount,
            request.Currency,
            request.TaxRatePercent), ct);

        return Ok(itemId);
    }

    [HttpPut("{id:guid}/service-items/{itemId:guid}")]
    public async Task<IActionResult> UpdateServiceItem(
        Guid id,
        Guid itemId,
        [FromBody] ServiceItemRequest request,
        CancellationToken ct)
    {
        await _sender.Send(new UpdateServiceItemCommand(
            id,
            itemId,
            request.Description,
            request.Hours,
            request.HourlyRateAmount,
            request.Currency,
            request.TaxRatePercent), ct);

        return NoContent();
    }

    [HttpDelete("{id:guid}/service-items/{itemId:guid}")]
    public async Task<IActionResult> RemoveServiceItem(Guid id, Guid itemId, CancellationToken ct)
    {
        await _sender.Send(new RemoveServiceItemCommand(id, itemId), ct);
        return NoContent();
    }
}

// ── Request body types ────────────────────────────────────────────────────────

public record UpdateProjectRequest(string Name, string? Description);

public record ServiceItemRequest(
    string Description,
    decimal Hours,
    decimal HourlyRateAmount,
    string Currency,
    int TaxRatePercent);
