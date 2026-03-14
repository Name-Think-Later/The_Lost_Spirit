using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Domain.Skill.Weave
{
    public record struct WeaveActivatedEvent(FormulaPayload Payload) : IDomainEvent;
}