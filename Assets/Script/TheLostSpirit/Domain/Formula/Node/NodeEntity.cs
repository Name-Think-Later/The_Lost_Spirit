using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Extension.General;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Formula.Node
{
    public class NodeEntity : IEntity<NodeID>
    {
        readonly Node _node;
        public NodeID ID { get; }

        public SkillID Skill {
            get => _node.Skill;
            set => _node.Skill = value;
        }

        public IEnumerable<Neighbor?> NullNeighbors =>
            _node
                .Neighbors
                .Where(n => !n.HasValue);

        public IEnumerable<Neighbor> NotNullNeighbors =>
            _node
                .Neighbors
                .Where(n => n.HasValue)
                .Cast<Neighbor>();

        public IEnumerable<Neighbor> OutNeighbors =>
            NotNullNeighbors
                .Where(n => n.IsOut);

        public NodeEntity(NodeID id, int neighborCount) {
            ID = id;

            _node = new Node(neighborCount);
        }

        public void Associate(int index, Neighbor neighbor) {
            Contract.Assert(index.InClosedInterval(0, _node.Neighbors.Count - 1), "Index is invalid");

            _node.Neighbors[index] = neighbor;
        }

        public void Cut(int index) {
            Contract.Assert(index.InClosedInterval(0, _node.Neighbors.Count - 1), "Index is invalid");

            _node.Neighbors[index] = null;
        }
    }
}