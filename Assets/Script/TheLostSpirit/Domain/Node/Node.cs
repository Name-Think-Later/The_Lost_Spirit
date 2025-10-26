using System.Collections.Generic;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Node {
    public class Node {
        public SkillID Skill { get; set; }
        public List<Neighbor?> Neighbors { get; } = new List<Neighbor?>();

        public Node(int neighborCount) {
            for (int i = 0; i < neighborCount; i++) Neighbors.Add(null);
        }
    }
}