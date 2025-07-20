namespace Script.TheLostSpirit.Circuit.CircuitNode {
    public partial class CircuitNode {
        public class Adjacency {
            public Adjacency(CircuitNode owner) {
                Owner = owner;
                Set(null);
            }

            public void Set(
                Adjacency     opposite,
                AdjacencyType type = AdjacencyType.In
            ) {
                Opposite = opposite;
                Type     = type;
            }

            public CircuitNode Owner { get; }
            public Adjacency Opposite { get; private set; }
            public AdjacencyType Type { get; private set; }
            public bool IsExist => Opposite != null;
            public bool IsEmpty => Opposite == null;
            public bool IsIn => IsExist && Type == AdjacencyType.In;
            public bool IsOut => IsExist && Type == AdjacencyType.Out;


            public void To(Adjacency target) {
                Set(target, AdjacencyType.Out);
                target.Set(this, AdjacencyType.In);
            }

            public void Cut() {
                Opposite.Set(null);
                Set(null);
            }
        }
    }

    public enum AdjacencyType {
        In,
        Out
    }
}