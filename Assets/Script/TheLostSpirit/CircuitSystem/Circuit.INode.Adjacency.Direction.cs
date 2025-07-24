using System.Collections.Generic;

namespace Script.TheLostSpirit.CircuitSystem {
    public partial class Circuit : IList<Circuit.INode> {
        public partial interface INode {
            public partial class Adjacency {
                public enum Direction {
                    In,
                    Out
                }
            }
        }
    }
}