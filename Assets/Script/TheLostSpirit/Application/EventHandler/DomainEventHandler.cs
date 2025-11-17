using R3;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Application.EventHandler {
    public abstract class DomainEventHandler<TEvent> where TEvent : IDomainEvent {
        protected DomainEventHandler() {
            AppScope
                .EventBus
                .ObservableEvent<TEvent>()
                .Subscribe(Handle);
        }

        protected abstract void Handle(TEvent domainEvent);
    }
}