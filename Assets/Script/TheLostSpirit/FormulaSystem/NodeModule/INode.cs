using Cysharp.Threading.Tasks;

namespace Script.TheLostSpirit.FormulaSystem.NodeModule {
    public interface INode {
        public AdjacencyList Adjacencies { get; }
        public UniTaskVoid AsyncableVisited();
    }
}