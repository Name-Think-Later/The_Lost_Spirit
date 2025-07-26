using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using MoreLinq;
using Script.TheLostSpirit.SkillSystem.CoreModule;
using Script.TheLostSpirit.SkillSystem.SkillBase;
using UnityEngine.WSA;

namespace Script.TheLostSpirit.CircuitSystem {
    public partial class Circuit : IList<Circuit.INode> {
        public class Node<T> : INode, Core.ICoreControllable where T : Skill {
            readonly T                   _skill;
            readonly INode.AdjacencyList _adjacencies;


            public Skill Skill => _skill;
            public INode.AdjacencyList Adjacencies => _adjacencies;

            /// <param name="skill">乘載的技能</param>
            /// <param name="adjacencyCount">枝度，預設為2</param>
            public Node(T skill, int adjacencyCount = 2) {
                _skill       = skill;
                _adjacencies = new INode.AdjacencyList(this, adjacencyCount);
            }

            /// <summary>
            /// Core specialize
            /// </summary>
            public Node(Core core, int adjacencyCount = 2)
                : this((T)(Skill)core, adjacencyCount) {
            }

            public void Activate() {
                ActivateAsync().Forget();
            }

            private async UniTaskVoid ActivateAsync() {
                await _skill.Activate();
                _adjacencies.Out.ForEach(a => {
                    a.Opposite.Owner.Activate();
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

                void PrintEachAdjance(INode.AdjacencyList adjacencyList) {
                    foreach (var item in adjacencyList) {
                        var name = item.Opposite == null ? "null" : item.Opposite.Owner.Skill.GetInfo.Name;
                        stringBuilder.AppendFormat($" -> [{name} | {item.GetDirection}]");
                    }
                }
            }

            public void CoreActivate() 
            {
                Activate();
            }
        }
    }
}