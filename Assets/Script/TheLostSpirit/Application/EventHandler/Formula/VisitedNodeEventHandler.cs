using System;
using Cysharp.Threading.Tasks;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Formula.Node;
using TheLostSpirit.Domain.Formula.Node.Event;
using UnityEngine;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class VisitedNodeEventHandler : AwaitableEventHandler<VisitedNodeEvent>
    {
        readonly NodeRepository _nodeRepository;

        public VisitedNodeEventHandler(NodeRepository nodeRepository) {
            _nodeRepository = nodeRepository;
        }

        protected override async UniTask Handle(VisitedNodeEvent domainEvent) {
            var nodeID     = domainEvent.NodeID;
            var nodeEntity = _nodeRepository.GetByID(nodeID);

            Debug.Log(nodeID);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await nodeEntity.MoveNext(TraversalPolicy.Parallel);
        }
    }
}