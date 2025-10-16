using System;
using Cysharp.Threading.Tasks;
using TheLostSpirit.Others.FormulaSystem.NodeModule;
using TheLostSpirit.Others.SkillSystem.SkillBase;
using TheLostSpirit.Others.SkillSystem.WeaveModule;

namespace TheLostSpirit.Others.FormulaSystem {
    public class SkillNode<T> : Node where T : Skill {
        readonly T    _skill;
        Func<UniTask> _visitorStrategy;


        public T Skill => _skill;

        /// <param name="skill">乘載的技能</param>
        /// <param name="adjacencyCount">枝度，預設為2</param>
        public SkillNode(T skill, int adjacencyCount = 2)
            : base(adjacencyCount) {
            _skill           = skill;
            _visitorStrategy = DefaultVisitorStrategy;
        }

        public SkillNode(Weave weave, int adjacencyCount = 2)
            : this((T)(Skill)weave, adjacencyCount) { }

        public override async UniTask AsyncVisited() {
            await _skill.Activate();
            await _visitorStrategy();
        }

        async UniTask DefaultVisitorStrategy() {
            await base.AsyncVisited();
        }
    }
}