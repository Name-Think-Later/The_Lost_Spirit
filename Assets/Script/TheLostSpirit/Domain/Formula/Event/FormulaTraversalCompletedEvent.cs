using System;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Domain.Formula
{
    public record struct FormulaTraversalCompletedEvent(Guid FormulaStreamID) : IDomainEvent;
}