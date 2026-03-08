using R3;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Application.EventHandler
{
    public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IDomainEvent
    {
        protected DomainEventHandler() {
            AppScope
                .EventBus
                .ObservableEvent<TEvent>()
                .Subscribe(Handle);
        }

        public abstract void Handle(TEvent domainEvent);
    }
}