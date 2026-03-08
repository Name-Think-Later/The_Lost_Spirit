using System;
using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Formula.Node.Event
{
    public record AsyncVisitedNodeEvent(NodeID NodeID, Guid SequentID, bool IsLastChild) : IAsyncDomainEvent
    {
        readonly UniTaskCompletionSource _completion = new UniTaskCompletionSource();

        public NodeID NodeID { get; } = NodeID;
        public Guid SequentID { get; } = SequentID;
        public bool IsLastChild { get; } = IsLastChild;

        public void Complete() {
            _completion.TrySetResult();
        }

        public UniTask Await() {
            return _completion.Task;
        }
    }
}