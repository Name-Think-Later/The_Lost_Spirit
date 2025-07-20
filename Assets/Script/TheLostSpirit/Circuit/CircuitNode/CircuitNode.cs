using System.Text;
using Script.TheLostSpirit.Circuit.Skill;

namespace Script.TheLostSpirit.Circuit.CircuitNode {
    public partial class CircuitNode {
        readonly SkillBase        _skill;
        readonly AdjacencyList _adjacencies;


        public AdjacencyList Adjacencies => _adjacencies;
        public int InDegree => _adjacencies.In.Count;
        public int OutDegree => _adjacencies.Out.Count;

        /// <param name="skill">乘載的記憶</param>
        /// <param name="adjacencyCount">枝度，預設為2</param>
        public CircuitNode(SkillBase skill, int adjacencyCount = 2) {
            _skill       = skill;
            _adjacencies = new AdjacencyList(this, adjacencyCount);
        }

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat($"{_skill.Info.Name} |");
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
                    var name = item.Opposite == null ? "null" : item.Opposite.Owner._skill.Info.Name;
                    stringBuilder.AppendFormat($" -> [{name} | {item.Type}]");
                }
            }
        }
    }
}