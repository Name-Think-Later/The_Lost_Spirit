using System;
using R3;
using TheLostSpirit.Others.SkillSystem.SkillBase;

namespace TheLostSpirit.Others.SkillSystem.CoreModule {
    public class Core : Skill {
        readonly CoreBehaviourData _behaviourData;

        IDisposable _disposable;

        public Core(CoreModel coreModel) : base(coreModel.Info) {
            _behaviourData = coreModel.BehaviourData;
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