using Cysharp.Threading.Tasks;
using Script.TheLostSpirit.FormulaSystem.NodeModule;
using Script.TheLostSpirit.SkillSystem.SkillBase;

namespace Script.TheLostSpirit.FormulaSystem {
    public class SkillNode<T> : INode where T : Skill {
        readonly T             _skill;
        readonly AdjacencyList _adjacencies;


        public T Skill => _skill;
        public AdjacencyList Adjacencies => _adjacencies;

        /// <param name="skill">乘載的技能</param>
        /// <param name="adjacencyCount">枝度，預設為2</param>
        public SkillNode(T skill, int adjacencyCount = 2) {
            _skill       = skill;
            _adjacencies = new AdjacencyList(this, adjacencyCount);
        }

        public async UniTaskVoid AsyncableVisited() {
            await _skill.Activate();
            foreach (var adjacency in _adjacencies.Out) {
                adjacency.Opposite.Owner.AsyncableVisited().Forget();
            }
        }
    }
}