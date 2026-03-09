using Cysharp.Threading.Tasks;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Formula.Node;
using TheLostSpirit.Domain.Formula.Node.Event;

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

            // Debug.Log(nodeID);
            payload.AddDebugRoute(nodeID);

            var nodeEntity = _nodeRepository.GetByID(nodeID);

            var activeSkillInput = new ActiveSkillUseCase.Input(nodeEntity.SkillID, payload);
            await _activeSkillUseCase.Execute(activeSkillInput);

            var notLeafNode = await nodeEntity.MoveNext(payload, TraversalPolicy.Parallel);

            if (notLeafNode) return;

            DestroyAnchors(payload);
            DestroyCandidateAnchors(payload);
        }

        void DestroyAnchors(FormulaPayload payload) {
            foreach (var anchorID in payload.Anchors) {
                var anchorEntity = _anchorRepository.TakeByID(anchorID);
                anchorEntity.Destroy();
            }

            payload.Anchors.Clear();
        }

        void DestroyCandidateAnchors(FormulaPayload payload) {
            foreach (var anchorID in payload.CandidateAnchors) {
                var anchorEntity = _anchorRepository.TakeByID(anchorID);
                anchorEntity.Destroy();
            }

            payload.CandidateAnchors.Clear();
        }
    }
}