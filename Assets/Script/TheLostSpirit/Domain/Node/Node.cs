using System.Collections.Generic;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Node
{
    public class Node
    {
        public SkillID Skill { get; set; }

        public List<Neighbor?> Neighbors { get; }

        public Node(int neighborCount) {
            Neighbors = new List<Neighbor?>();
            for (int i = 0; i < neighborCount; i++) Neighbors.Add(null);
        }
    }
}