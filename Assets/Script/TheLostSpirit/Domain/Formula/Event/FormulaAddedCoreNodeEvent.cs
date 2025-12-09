using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Formula.Event
{
    public record struct FormulaAddedCoreNodeEvent(FormulaID FormulaID, CoreID CoreID) : IDomainEvent;
}