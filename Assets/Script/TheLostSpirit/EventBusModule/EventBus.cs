using System;
using R3;

namespace Script.TheLostSpirit.EventBusModule {
    public static class EventBus {
        static readonly Subject<DomainEvent> _subject = new Subject<DomainEvent>();

        public static IDisposable Subscribe<T>(Action<T> callback) where T : DomainEvent {
            return _subject.OfType<DomainEvent, T>().Subscribe(callback);
        }

        public static IDisposable Subscribe<T>(Subject<T> subject) where T : DomainEvent {
            return _subject.OfType<DomainEvent, T>().Subscribe(subject.AsObserver());
        }


        public static void Publish<T>() where T : DomainEvent, new() {
            _subject.OnNext(new T());
        }

        public static void Publish<T>(T domainEvent) where T : DomainEvent {
            _subject.OnNext(domainEvent);
        }
    }
}