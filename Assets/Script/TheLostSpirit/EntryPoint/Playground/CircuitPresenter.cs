using R3;
using Script.TheLostSpirit.CircuitSystem;
using Script.TheLostSpirit.EventBusModule;

namespace Script.TheLostSpirit.EntryPoint.Playground {
    public class CircuitPresenter {
        public CircuitPresenter(Circuit[] circuits) {
            EventBus.ObservableEvent<CircuitTraversalEvent>().Subscribe(e => {
                circuits[e.Index].Traverse();
            });

        }
    }
}