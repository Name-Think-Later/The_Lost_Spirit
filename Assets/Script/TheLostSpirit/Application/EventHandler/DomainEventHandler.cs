using R3;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Application.EventHandler
{
    public abstract class DomainEventHandler<TEvent> where TEvent : IDomainEvent
    {
        protected DomainEventHandler() {
            AppScope
                .EventBus
                .ObservableEvent<TEvent>()
                .Subscribe(Handle);
        }

        protected abstract void Handle(TEvent domainEvent);
    }
}