using Cysharp.Threading.Tasks;
using Script.TheLostSpirit.Circuit.SkillSystem.SkillBase;

namespace Script.TheLostSpirit.Circuit.SkillSystem.CoreModule {
    public partial class Core : Skill {
        Core.BehaviourData _behaviourData;

        public Core(Core.Model model) : base(model.Info) {
            _behaviourData = model.BehaviourData;
        }

        public override async UniTask Activate() {
            await base.Activate();
        }
    }
}