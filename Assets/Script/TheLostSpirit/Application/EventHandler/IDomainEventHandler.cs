using System;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Application.EventHandler
{
    public interface IDomainEventHandler<in TEvent> : IDisposable where TEvent : IDomainEvent
    {
        protected void Handle(TEvent domainEvent);
    }
}