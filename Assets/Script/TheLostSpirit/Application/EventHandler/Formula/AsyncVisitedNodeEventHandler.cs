using System.Linq;
using Cysharp.Threading.Tasks;
using MoreLinq;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Formula.Node;
using TheLostSpirit.Domain.Formula.Node.Event;
using TheLostSpirit.Domain.Skill.Anchor;
using UnityEngine;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class AsyncVisitedNodeEventHandler : AsyncDomainEventHandler<AsyncVisitedNodeEvent>
    {
        readonly NodeRepository     _nodeRepository;
        readonly AnchorRepository   _anchorRepository;
        readonly ActiveSkillUseCase _activeSkillUseCase;

        public AsyncVisitedNodeEventHandler(
            NodeRepository     nodeRepository,
            AnchorRepository   anchorRepository,
            ActiveSkillUseCase activeSkillUseCase
        ) {
            _nodeRepository     = nodeRepository;
            _anchorRepository   = anchorRepository;
            _activeSkillUseCase = activeSkillUseCase;
        }

        public override async UniTask Handle(AsyncVisitedNodeEvent domainEvent) {
            var nodeID  = domainEvent.NodeID;
            var payload = domainEvent.Payload;

            Debug.Log(nodeID.Index);
            payload.AddRoute(nodeID);

            var nodeEntity = _nodeRepository.GetByID(nodeID);

            var activeSkillInput = new ActiveSkillUseCase.Input(nodeEntity.SkillID, payload);
            await _activeSkillUseCase.Execute(activeSkillInput);

            await nodeEntity.MoveNext(payload, TraversalPolicy.Sequential);
        }
    }
}