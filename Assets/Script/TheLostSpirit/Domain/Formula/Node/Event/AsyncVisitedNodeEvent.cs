using System;
using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Formula.Node.Event
{
    public record AsyncVisitedNodeEvent(
        NodeID         NodeID,
        FormulaPayload Payload
    ) : IAsyncDomainEvent
    {
        readonly UniTaskCompletionSource _completion = new UniTaskCompletionSource();

        public NodeID NodeID { get; } = NodeID;
        public FormulaPayload Payload { get; } = Payload;

        public void Complete() {
            _completion.TrySetResult();
        }

        public UniTask Await() {
            return _completion.Task;
        }
    }
}