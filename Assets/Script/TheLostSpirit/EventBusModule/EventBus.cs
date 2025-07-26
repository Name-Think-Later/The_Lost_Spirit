using System;
using R3;

namespace Script.TheLostSpirit.EventBusModule {
    public static class EventBus {
        static readonly Subject<DomainEvent> _subject = new Subject<DomainEvent>();
        
        public static Observable<T> ObservableEvent<T>() {
            return _subject.OfType<DomainEvent, T>();
        }

        public static void Publish<T>() where T : DomainEvent, new() {
            _subject.OnNext(new T());
        }

        public static void Publish<T>(T domainEvent) where T : DomainEvent {
            _subject.OnNext(domainEvent);
        }
    }
}