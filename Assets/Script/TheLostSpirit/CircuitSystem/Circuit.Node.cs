using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using MoreLinq;
using Script.TheLostSpirit.SkillSystem.SkillBase;

namespace Script.TheLostSpirit.CircuitSystem {
    public partial class Circuit : IList<Circuit.Node> {
        public partial class Node {
            readonly Skill         _skill;
            readonly AdjacencyList _adjacencies;


            public AdjacencyList Adjacencies => _adjacencies;
            public int InDegree => _adjacencies.In.Count;
            public int OutDegree => _adjacencies.Out.Count;

            /// <param name="skill">乘載的技能</param>
            /// <param name="adjacencyCount">枝度，預設為2</param>
            public Node(Skill skill, int adjacencyCount = 2) {
                _skill       = skill;
                _adjacencies = new AdjacencyList(this, adjacencyCount);
            }


            public void Traversal() {
                TraversalAsync().Forget();
            }

            private async UniTaskVoid TraversalAsync() {
                await _skill.Activate();
                _adjacencies.Out.ForEach(a => {
                    a.Opposite.Owner.Traversal();
                });
            }

            public override string ToString() {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendFormat($"{_skill.GetInfo.Name} |");
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
                        var name = item.Opposite == null ? "null" : item.Opposite.Owner._skill.GetInfo.Name;
                        stringBuilder.AppendFormat($" -> [{name} | {item.GetDirection}]");
                    }
                }
            }
        }
    }
}