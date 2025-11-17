using System.Collections.Generic;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Formula
{
    public class Formula
    {
        public List<NodeID> Nodes { get; }
        public NodeID CoreNode { get; set; }

        public Formula() {
            Nodes = new List<NodeID>();
        }
    }
}