namespace Script.Circuit {
    public partial class CircuitNode {
        public class Adjacency {
            public Adjacency() {
                Set(null);
            }

            public void Set(
                CircuitNode    node,
                AdjacencyState state = AdjacencyState.In
            ) {
                Node  = node;
                State = state;
            }

            public CircuitNode Node { get; private set; }
            public AdjacencyState State { get; private set; }
            public bool IsExist => Node != null;
            public bool IsEmpty => Node == null;
            public bool IsIn => IsExist && State == AdjacencyState.In;
            public bool IsOut => IsExist && State == AdjacencyState.Out;
        }
    }

    public enum AdjacencyState {
        In,
        Out
    }
}