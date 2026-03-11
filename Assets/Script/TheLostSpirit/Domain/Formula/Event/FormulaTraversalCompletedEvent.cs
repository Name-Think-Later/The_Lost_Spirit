using System;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Domain.Formula.Event
{
    public record struct FormulaTraversalCompletedEvent(Guid FormulaStreamID) : IDomainEvent;
}