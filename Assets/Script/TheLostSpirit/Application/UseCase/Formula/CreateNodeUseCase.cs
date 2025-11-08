using System.Linq;
using MoreLinq;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Node;
using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class CreateNodeUseCase : IUseCase<CreateNodeUseCase.Output, CreateNodeUseCase.Input>
    {
        readonly NodeRepository _nodeRepository;

        public CreateNodeUseCase(NodeRepository nodeRepository) {
            _nodeRepository = nodeRepository;
        }

        public Output Execute(Input input) {
            var nodeID = NodeID.New();

            var nodeEntity = new NodeEntity(nodeID, input.NeighborCount);
            _nodeRepository.Save(nodeEntity);

            return new Output(nodeID);
        }

        public record struct Input(int NeighborCount) : IInput;

        public record struct Output(NodeID NodeID) : IOutput;
    }
}