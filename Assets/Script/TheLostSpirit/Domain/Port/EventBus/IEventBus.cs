using Cysharp.Threading.Tasks;
using R3;

namespace TheLostSpirit.Domain.Port.EventBus
{
    public interface IEventBus
    {
        public Observable<TEvent> ObservableEvent<TEvent>() where TEvent : IDomainEvent;

        public void Publish<TEvent>() where TEvent : IDomainEvent, new();

        public void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;

        public UniTask PublishAsync<TEvent>() where TEvent : IAsyncDomainEvent, new();

        public UniTask PublishAsync<TEvent>(TEvent domainEvent) where TEvent : IAsyncDomainEvent;
    }
}