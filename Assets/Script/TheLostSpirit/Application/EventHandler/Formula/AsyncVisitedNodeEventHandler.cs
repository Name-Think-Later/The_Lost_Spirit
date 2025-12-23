using Cysharp.Threading.Tasks;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Domain.Formula.Node;
using TheLostSpirit.Domain.Formula.Node.Event;
using UnityEngine;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class AsyncVisitedNodeEventHandler : AsyncDomainEventHandler<AsyncVisitedNodeEvent>
    {
        readonly ActiveSkillUseCase _activeSkillUseCase;
        readonly NodeRepository     _nodeRepository;

        public AsyncVisitedNodeEventHandler(
            NodeRepository     nodeRepository,
            ActiveSkillUseCase activeSkillUseCase
        ) {
            _nodeRepository     = nodeRepository;
            _activeSkillUseCase = activeSkillUseCase;
        }

        protected override async UniTask Handle(AsyncVisitedNodeEvent domainEvent) {
            var nodeID  = domainEvent.NodeID;
            var payload = domainEvent.Payload;

            // Debug.Log(nodeID);
            payload.AddDebugRoute(nodeID);

            var nodeEntity = _nodeRepository.GetByID(nodeID);

            var activeSkillInput = new ActiveSkillUseCase.Input(nodeEntity.Skill, payload);
            await _activeSkillUseCase.Execute(activeSkillInput);

            payload.PushAnchors();

            await nodeEntity.MoveNext(payload, TraversalPolicy.Parallel);
        }
    }
}