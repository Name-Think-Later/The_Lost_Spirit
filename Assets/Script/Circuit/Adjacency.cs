using UnityEngine;

namespace Script.Circuit {
    public partial class CircuitNode {
        public class Adjacency {
            readonly CircuitNode _owner;

            public Adjacency(CircuitNode owner) {
                _owner = owner;
                Set(null);
            }

            public void Set(
                Adjacency      opposite,
                AdjacencyState state = AdjacencyState.In
            ) {
                Opposite = opposite;
                State    = state;
            }

            public CircuitNode Owner => _owner;
            public Adjacency Opposite { get; private set; }
            public AdjacencyState State { get; private set; }
            public bool IsExist => Opposite != null;
            public bool IsEmpty => Opposite == null;
            public bool IsIn => IsExist && State == AdjacencyState.In;
            public bool IsOut => IsExist && State == AdjacencyState.Out;


            public void To(Adjacency target) {
                /*
                //target是否為自身
                if (this.Equals(target)) return false;

                //是否已連接target
                //if (AdjacencyEmpty.ContainsValue(target)) return false;
                if (_adjacencies.Exist.Contains(target))

                    //索引是否正在被使用
                    //if (!AdjacencyExist.ContainsKey(myIndex))
                    if (_adjacencies[myIndex].IsExist)
                        return false;

                //target索引突觸是否可用
                //if (!target.AdjacencyExist.ContainsKey(targetIndex)) return false;

                if (!target._adjacencies[targetIndex].IsExist) return false;

                */
                Set(target, AdjacencyState.Out);
                target.Set(this, AdjacencyState.In);
            }

            public void Cut() {
                Opposite.Set(null);
                Set(null);
            }
        }
    }

    public enum AdjacencyState {
        In,
        Out
    }
}