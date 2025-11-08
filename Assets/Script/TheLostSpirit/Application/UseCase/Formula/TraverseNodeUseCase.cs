using System.Linq;
using MoreLinq;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Application.UseCase.Formula
{
    public class TraverseNodeUseCase : IUseCase<Void, TraverseNodeUseCase.Input>
    {
        readonly FormulaRepository _formulaRepository;
        readonly NodeRepository    _nodeRepository;

        public TraverseNodeUseCase(
            NodeRepository    nodeRepository,
            FormulaRepository formulaRepository
        ) {
            _nodeRepository    = nodeRepository;
            _formulaRepository = formulaRepository;
        }

        public Void Execute(Input input) {
            var formulaEntity = _formulaRepository.GetByID(input.FormulaID);
            Traverse(formulaEntity.CoreNode.nodeID);

            return Void.Default;
        }

        void Traverse(NodeID nodeID) {
            Debug.Log(nodeID);
            var nodeEntity = _nodeRepository.GetByID(nodeID);
            nodeEntity.OutNeighbors.Select(n => n.ID).ForEach(Traverse);
        }

        public record struct Input(FormulaID FormulaID) : IInput;
    }
}