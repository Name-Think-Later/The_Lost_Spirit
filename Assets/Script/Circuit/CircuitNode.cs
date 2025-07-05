using System.Collections;
using System.Collections.Generic;
using System.Text;
using ZLinq;

namespace Script.Circuit {
    public partial class CircuitNode {
        readonly Memory        _memory;
        readonly AdjacencyList _adjacencies;


        public AdjacencyList Adjacencies => _adjacencies;
        public int InDegree => _adjacencies.In.Count;
        public int OutDegree => _adjacencies.Out.Count;

        /// <param name="memory">乘載的記憶</param>
        /// <param name="adjacencyCount">枝度，預設為2</param>
        public CircuitNode(Memory memory, int adjacencyCount = 2) {
            _memory      = memory;
            _adjacencies = new AdjacencyList(this, adjacencyCount);
        }

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat($"{_memory.Name} |");
            PrintEachAdjance(_adjacencies);

            stringBuilder.Append("\n");

            stringBuilder.Append("In |");
            PrintEachAdjance(_adjacencies.In);
            stringBuilder.Append("\n");

            stringBuilder.Append("Out |");
            PrintEachAdjance(_adjacencies.Out);

            return stringBuilder.ToString();

            void PrintEachAdjance(AdjacencyList adjacencyList) {
                foreach (var item in adjacencyList) {
                    var name = item.Opposite == null ? "null" : item.Opposite.Owner._memory.Name;
                    stringBuilder.AppendFormat($" -> [{name} | {item.State}]");
                }
            }
        }
    }
}