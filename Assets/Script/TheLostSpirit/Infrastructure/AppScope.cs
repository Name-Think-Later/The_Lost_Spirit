using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Infrastructure {
    public static class AppScope {
        public static EventBus EventBus { get; } = new EventBus();
    }
}