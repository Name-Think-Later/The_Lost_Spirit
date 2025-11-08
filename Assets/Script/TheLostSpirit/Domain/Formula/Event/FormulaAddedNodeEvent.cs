using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Formula.Event
{
    public record struct FormulaAddedNodeEvent(FormulaID FormulaID, NodeID NodeID) : IDomainEvent;
}