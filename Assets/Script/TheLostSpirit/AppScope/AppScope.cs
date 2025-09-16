using Script.TheLostSpirit.EventBusModule;

namespace Script.TheLostSpirit.AppScope {
    public class AppScope {
        EventBus EventBus { get; }

        public AppScope() {
            EventBus = new EventBus();
        }
    }
}