using System;
using Script.TheLostSpirit.SkillSystem.SkillBase;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    public partial class Core : Skill {
        readonly BehaviourData _behaviourData;

        IDisposable _disposable;

        public Core(Model model) : base(model.Info) {
            _behaviourData = model.BehaviourData;
        }

        public void Initialize(ICoreControllable controllable) {
            var circuitActivator = _behaviourData.CircuitActivator;
            var activeInput      = controllable.GetActiveInput();

            var activateObservable = circuitActivator.GetObservableActivator(activeInput);

            _disposable = controllable.ApplyActivator(activateObservable);
        }
    }
}