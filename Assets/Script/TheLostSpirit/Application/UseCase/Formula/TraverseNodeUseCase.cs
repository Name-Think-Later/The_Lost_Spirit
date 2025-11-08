using System.Linq;
using MoreLinq;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class TraverseNodeUseCase : IUseCase<Void, TraverseNodeUseCase.Input>
    {
        readonly NodeRepository _nodeRepository;

        public TraverseNodeUseCase(NodeRepository nodeRepository) {
            _nodeRepository = nodeRepository;
        }

        public Void Execute(Input input) {
            Traverse(input.Head);

            return Void.Default;
        }

        void Traverse(NodeID nodeID) {
            var nodeEntity = _nodeRepository.GetByID(nodeID);
            Debug.Log(nodeID);
            nodeEntity.OutNeighbors.Select(n => n.ID).ForEach(Traverse);
        }

        public record struct Input(NodeID Head) : IInput;
    }
}