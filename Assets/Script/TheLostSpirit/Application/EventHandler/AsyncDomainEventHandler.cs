using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Application.EventHandler
{
    public abstract class AsyncDomainEventHandler<TEvent> : IAsyncDomainEventHandler<TEvent>
        where TEvent : IAsyncDomainEvent
    {
        readonly IDisposable _disposable;

        protected AsyncDomainEventHandler() {
            //AwaitOperation.Sequential is FIFO
            //Recursion will conflict the rule
            _disposable =
                AppScope
                    .EventBus
                    .ObservableEvent<TEvent>()
                    .SubscribeAwait(
                        async (domainEvent, token) => {
                            await Handle(domainEvent);
                            domainEvent.Complete();
                        },
                        AwaitOperation.Parallel
                    );
        }

        public abstract UniTask Handle(TEvent domainEvent);

        public void Dispose() {
            _disposable.Dispose();
        }
    }
}