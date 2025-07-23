using System.Collections.Generic;

namespace Script.TheLostSpirit.CircuitSystem {
    public partial class Circuit : IList<Circuit.Node> {
        public partial class Node {
            public partial class Adjacency {
                public enum Direction {
                    In,
                    Out
                }
            }
        }
    }
}