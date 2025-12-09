using System.Collections.Generic;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Formula.Node
{
    public class Node
    {
        public Node(int neighborCount) {
            Neighbors = new List<Neighbor?>();
            for (var i = 0; i < neighborCount; i++) Neighbors.Add(null);
        }

        public ISkillID Skill { get; set; }

        public List<Neighbor?> Neighbors { get; }
    }
}