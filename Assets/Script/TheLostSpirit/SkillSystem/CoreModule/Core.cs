using Cysharp.Threading.Tasks;
using Script.TheLostSpirit.SkillSystem.SkillBase;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    public partial class Core : Skill {
        readonly Core.BehaviourData _behaviourData;

        public Core(Core.Model model) : base(model.Info) {
            _behaviourData = model.BehaviourData;
        }

        public void Initialize(ICoreControllable node) {
            
        }

        public override async UniTask Activate() {
            await base.Activate();
        }
    }
}