using System.Collections.Generic;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Node {
    public class NodeEntity : IEntity<NodeID> {
        readonly Node _node;
        public NodeID ID { get; }

        public IReadOnlyList<Neighbor?> Neighbors => _node.Neighbors;

        public NodeEntity(NodeID id, int neighborCount) {
            ID = id;

            _node = new Node(neighborCount);
        }


        public NodeEntity WithSkill(SkillID skill) {
            _node.Skill = skill;

            return this;
        }

        public void Associate(int index, NodeID id, AssociateType type) {
            _node.Neighbors[index] = new Neighbor(id, type);
        }

        public void Cut(int index) {
            _node.Neighbors[index] = null;
        }
    }
}