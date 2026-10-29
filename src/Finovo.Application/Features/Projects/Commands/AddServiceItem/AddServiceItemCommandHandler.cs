using Finovo.Application.Common.Exceptions;
using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Enums;
using Finovo.Domain.ValueObjects;
using MediatR;

namespace Finovo.Application.Features.Projects.Commands.AddServiceItem;

public sealed class AddServiceItemCommandHandler : IRequestHandler<AddServiceItemCommand, Guid>
{
    private readonly IProjectRepository _repository;

    public AddServiceItemCommandHandler(IProjectRepository repository)
        => _repository = repository;

    public async Task<Guid> Handle(AddServiceItemCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Projects.Project), request.ProjectId);

        var hourlyRate = new Money(request.HourlyRateAmount, request.Currency);
        var taxRate = (TaxRate)request.TaxRatePercent;

        var item = project.AddServiceItem(request.Description, request.Hours, hourlyRate, taxRate);
        await _repository.SaveChangesAsync(cancellationToken);

        return item.Id;
    }
}
