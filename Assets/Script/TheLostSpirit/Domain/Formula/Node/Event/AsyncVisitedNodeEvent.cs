using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Port;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Formula.Node.Event
{
    public record AsyncVisitedNodeEvent(NodeID NodeID, FormulaPayload Payload) : IAsyncDomainEvent
    {
        readonly UniTaskCompletionSource _completion = new();

        public NodeID NodeID { get; } = NodeID;
        public FormulaPayload Payload { get; } = Payload;
        public void Complete() => _completion.TrySetResult();
        public UniTask Await() => _completion.Task;
        public AsyncVisitedNodeEvent(NodeID NodeID) : this(NodeID, new FormulaPayload()) { }
    }
}