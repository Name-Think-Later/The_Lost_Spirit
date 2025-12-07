using System.Collections.Generic;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Formula.Node
{
    public class Node
    {
        public ISkillID Skill { get; set; }

        public List<Neighbor?> Neighbors { get; }

        public Node(int neighborCount) {
            Neighbors = new List<Neighbor?>();
            for (int i = 0; i < neighborCount; i++) Neighbors.Add(null);
        }
    }
}