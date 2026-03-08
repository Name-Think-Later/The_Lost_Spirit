using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Application.EventHandler
{
    public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        protected void Handle(TEvent domainEvent);
    }
}