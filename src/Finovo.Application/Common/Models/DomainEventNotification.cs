using Finovo.Domain.Common;
using MediatR;

namespace Finovo.Application.Common.Models;

public record DomainEventNotification(IDomainEvent DomainEvent) : INotification;
