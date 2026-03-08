using System;
using R3;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Application.EventHandler
{
    public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<TEvent>
        where TEvent : IDomainEvent
    {
        readonly IDisposable _disposable;

        protected DomainEventHandler() {
            _disposable =
                AppScope
                    .EventBus
                    .ObservableEvent<TEvent>()
                    .Subscribe(Handle);
        }

        public abstract void Handle(TEvent domainEvent);

        public void Dispose() {
            _disposable.Dispose();
        }
    }
}