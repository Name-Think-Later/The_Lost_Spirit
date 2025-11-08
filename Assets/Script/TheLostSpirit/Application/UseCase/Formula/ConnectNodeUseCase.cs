using MoreLinq;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Formula.Node;
using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class ConnectNodeUseCase : IUseCase<Void, ConnectNodeUseCase.Input>
    {
        readonly NodeRepository _nodeRepository;

        public ConnectNodeUseCase(NodeRepository nodeRepository) {
            _nodeRepository = nodeRepository;
        }

        public Void Execute(Input input) {
            var from = input.From;
            var to   = input.To;

            var nodeFrom = _nodeRepository.GetByID(from.nodeID);
            var nodeTo   = _nodeRepository.GetByID(to.nodeID);

            var neighborOut = new Neighbor(to.nodeID, AssociateType.Out);
            nodeFrom.Associate(from.index, neighborOut);

            var neighborIn = new Neighbor(from.nodeID, AssociateType.In);
            nodeTo.Associate(to.index, neighborIn);

            return Void.Default;
        }

        public record struct Input((NodeID nodeID, int index) From, (NodeID nodeID, int index) To) : IInput;
    }
}