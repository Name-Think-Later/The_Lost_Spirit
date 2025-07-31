using System.Collections.Generic;

namespace Script.TheLostSpirit.CircuitSystem {
    partial class Circuit {
        partial interface INode {
            partial class Adjacency {
                public enum Direction {
                    In,
                    Out
                }
            }
        }
    }
}