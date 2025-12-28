using System.Collections.Generic;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Formula
{
    public class Formula
    {
        public Formula()
        {
            Nodes = new List<NodeID>();
        }

        public List<NodeID> Nodes { get; }
        public NodeID CoreNode { get; set; }
    }
}