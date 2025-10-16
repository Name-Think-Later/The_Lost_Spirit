using R3;

namespace TheLostSpirit.Infrastructure.EventDriven {
    public class EventBus {
        readonly Subject<IDomainEvent> _subject = new Subject<IDomainEvent>();

        public Observable<T> ObservableEvent<T>() {
            return _subject.OfType<IDomainEvent, T>();
        }

        public void Publish<T>() where T : IDomainEvent, new() {
            _subject.OnNext(new T());
        }

        public void Publish<T>(T domainEvent) where T : IDomainEvent {
            _subject.OnNext(domainEvent);
        }
    }
}