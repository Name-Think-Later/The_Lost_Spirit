using R3;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Application.EventHandler {
    public abstract class DomainEventHandler<T> where T : IDomainEvent {
        protected DomainEventHandler() {
            AppScope
                .EventBus
                .ObservableEvent<T>()
                .Subscribe(Handle);
        }

        protected abstract void Handle(T domainEvent);
    }
}