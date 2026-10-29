using Finovo.Domain.Common;

namespace Finovo.Domain.Events;

public record ProjectCompletedDomainEvent(
    Guid ProjectId,
    Guid TenantId) : IDomainEvent;
