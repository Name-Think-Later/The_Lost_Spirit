using System;
using System.Diagnostics.Contracts;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace TheLostSpirit.Infrastructure.EventDriven
{
    public class EventBus
    {
        readonly Subject<IDomainEvent> _subject = new Subject<IDomainEvent>();

        public Observable<T> ObservableEvent<T>() {
            //OfType is the constraint from subscriber
            return _subject.OfType<IDomainEvent, T>();
        }

        public void Publish<T>() where T : IDomainEvent, new() {
            _subject.OnNext(new T());
        }

        public void Publish<T>(T domainEvent) where T : IDomainEvent {
            _subject.OnNext(domainEvent);
        }

        public async UniTask PublishAwait<T>(
            T                 domainEvent,
            CancellationToken ct = default
        ) where T : IAwaitableDomainEvent {
            Contract.Assert(domainEvent.Completion != null, "Completion is null");

            _subject.OnNext(domainEvent);

            var waitTask = domainEvent.Completion.Task.AttachExternalCancellation(ct);

            await waitTask;
        }
    }
}