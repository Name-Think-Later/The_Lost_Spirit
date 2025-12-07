using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Domain
{
    public static class AppScope
    {
        public static IEventBus EventBus { get; private set; }

        public static void Initialize(IEventBus eventBus) {
            EventBus = eventBus;
        }
    }
}