using System;
using JetBrains.Annotations;
using R3;
using UnityEngine;

namespace Script {
    public class EventBus {
        readonly Subject<DomainEvent> _subject = new Subject<DomainEvent>();

        public IDisposable Register<T>(Action<T> callback) where T : DomainEvent {
            return _subject.OfType<DomainEvent, T>().Subscribe(callback);
        }

        public void Publish<T>() where T : DomainEvent, new() {
            _subject.OnNext(new T());
        }

        public void Publish<T>(T domainEvent) where T : DomainEvent {
            _subject.OnNext(domainEvent);
        }
    }
}
