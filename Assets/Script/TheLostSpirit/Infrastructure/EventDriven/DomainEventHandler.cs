using R3;

namespace TheLostSpirit.Infrastructure.EventDriven {
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