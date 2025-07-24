using System.Collections.Generic;

namespace Script.TheLostSpirit.CircuitSystem {
    public partial class Circuit : IList<Circuit.INode> {
        public partial interface INode {
            public partial class Adjacency {
                public Adjacency(INode owner) {
                    Owner = owner;
                    Set(null);
                }

                public void Set(
                    Adjacency opposite,
                    Direction direction = Direction.In
                ) {
                    Opposite     = opposite;
                    GetDirection = direction;
                }

                public INode Owner { get; }
                public Adjacency Opposite { get; private set; }
                public Direction GetDirection { get; private set; }
                public bool IsExist => Opposite != null;
                public bool IsEmpty => Opposite == null;
                public bool IsIn => IsExist && GetDirection == Direction.In;
                public bool IsOut => IsExist && GetDirection == Direction.Out;


                public void To(Adjacency target) {
                    Set(target, Direction.Out);
                    target.Set(this, Direction.In);
                }

                public void Cut() {
                    Opposite.Set(null);
                    Set(null);
                }
            }
        }
    }
}