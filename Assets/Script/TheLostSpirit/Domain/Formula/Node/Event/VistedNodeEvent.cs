using Cysharp.Threading.Tasks;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Domain.Formula.Node.Event
{
    public record struct VisitedNodeEvent(NodeID NodeID) : IAwaitableDomainEvent
    {
        public UniTaskCompletionSource Completion { get; } = new UniTaskCompletionSource();
    }
}