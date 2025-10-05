using R3;

namespace TheLostSpirit.Infrastructure.EventDriven {
    public class EventBus {
        readonly Subject<DomainEvent> _subject = new Subject<DomainEvent>();

        public Observable<T> ObservableEvent<T>() {
            return _subject.OfType<DomainEvent, T>();
        }

        public void Publish<T>() where T : DomainEvent, new() {
            _subject.OnNext(new T());
        }

        public void Publish<T>(T domainEvent) where T : DomainEvent {
            _subject.OnNext(domainEvent);
        }
    }
}