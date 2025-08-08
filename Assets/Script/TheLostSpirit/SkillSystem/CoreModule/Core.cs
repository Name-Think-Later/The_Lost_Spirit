using System;
using Script.TheLostSpirit.SkillSystem.SkillBase;
using R3;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    public partial class Core : Skill {
        readonly BehaviourData _behaviourData;

        IDisposable _disposable;

        public Core(Model model) : base(model.Info) {
            _behaviourData = model.BehaviourData;
        }

        public void Initialize(ICoreControllable formula) {
            var inputHandler  = _behaviourData.InputHandler;
            var outputHandler = _behaviourData.OutputHandler;
            var activeInput   = formula.GetActiveInput;

            var activateObservable = inputHandler.CreateObservableActivator(activeInput);
            outputHandler.OutputAction = formula.Activate;

            _disposable = activateObservable.Subscribe(_ => outputHandler.HandleOutput());
        }
    }
}