using System.Collections.Generic;
using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Formula
{
    public class Formula
    {
        public List<NodeID> Nodes { get; }
        public (NodeID nodeID, CoreID coreID) HeadNode { get; set; }

        public Formula() {
            Nodes = new List<NodeID>();
        }
    }
}