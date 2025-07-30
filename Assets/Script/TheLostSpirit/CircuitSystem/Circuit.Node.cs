using System.Text;
using Cysharp.Threading.Tasks;
using MoreLinq;
using Script.TheLostSpirit.SkillSystem.SkillBase;
using UnityEngine;

namespace Script.TheLostSpirit.CircuitSystem {
    public partial class Circuit {
        public class SkillNode<T> : INode where T : Skill {
            readonly T                   _skill;
            readonly INode.AdjacencyList _adjacencies;


            public T Skill => _skill;
            public INode.AdjacencyList Adjacencies => _adjacencies;

            /// <param name="skill">乘載的技能</param>
            /// <param name="adjacencyCount">枝度，預設為2</param>
            public SkillNode(T skill, int adjacencyCount = 2) {
                _skill       = skill;
                _adjacencies = new INode.AdjacencyList(this, adjacencyCount);
            }

            public async UniTaskVoid AsyncActivate() {
                await _skill.Activate();
                foreach (var adjacency in _adjacencies.Out) {
                    adjacency.Opposite.Owner.AsyncActivate().Forget();
                }
            }
        }
    }
}