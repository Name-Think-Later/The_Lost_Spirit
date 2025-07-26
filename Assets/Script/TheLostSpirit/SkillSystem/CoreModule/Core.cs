using Cysharp.Threading.Tasks;
using R3;
using Script.TheLostSpirit.EventBusModule;
using Script.TheLostSpirit.SkillSystem.SkillBase;
using UnityEngine;

namespace Script.TheLostSpirit.SkillSystem.CoreModule {
    public partial class Core : Skill {
        readonly Core.BehaviourData _behaviourData;

        public Core(Core.Model model) : base(model.Info) {
            _behaviourData = model.BehaviourData;
        }

        public void Initialize() {

            //process TraversalInputEvent
            EventBus
                .ObservableEvent<TraversalInputEvent.PerformedEvent>()
                .Subscribe(e  => {
                    EventBus.Publish(new CircuitTraversalEvent(e.Index));
                });


        }

        public override async UniTask Activate() {
            await base.Activate();
        }
    }
}