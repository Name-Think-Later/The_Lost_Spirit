using Cysharp.Threading.Tasks;
using R3;
using TheLostSpirit.Domain.Port;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Infrastructure
{
    public class EventBus : IEventBus
    {
        readonly Subject<IDomainEvent> _subject = new();

        public Observable<TEvent> ObservableEvent<TEvent>() where TEvent : IDomainEvent {
            //OfType is the constraint from subscriber
            return _subject.OfType<IDomainEvent, TEvent>();
        }

        public void Publish<TEvent>() where TEvent : IDomainEvent, new() {
            Publish(new TEvent());
        }

        public void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent {
            _subject.OnNext(domainEvent);
        }

        public UniTask PublishAsync<TEvent>() where TEvent : IAsyncDomainEvent, new() {
            return PublishAsync(new TEvent());
        }

        public UniTask PublishAsync<TEvent>(TEvent domainEvent) where TEvent : IAsyncDomainEvent {
            Publish(domainEvent);

            return domainEvent.Await();
        }
    }
}