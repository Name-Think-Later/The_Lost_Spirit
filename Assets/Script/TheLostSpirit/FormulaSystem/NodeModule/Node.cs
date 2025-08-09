using Cysharp.Threading.Tasks;

namespace Script.TheLostSpirit.FormulaSystem.NodeModule {
    public class Node {
        readonly AdjacencyList _adjacencies;
        public AdjacencyList Adjacencies => _adjacencies;

        public Node(int adjacencyCount) {
            _adjacencies = new AdjacencyList(this, adjacencyCount);
        }

        public virtual async UniTask AsyncVisited() {
            foreach (var adjacency in _adjacencies.Out) {
                await adjacency.Opposite.Owner.AsyncVisited();
            }
        }
    }
}