using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Application.EventHandler
{
    public abstract class AsyncDomainEventHandler<TEvent> : IAsyncDomainEventHandler<TEvent>
        where TEvent : IAsyncDomainEvent
    {
        protected AsyncDomainEventHandler() {
            var token = CancellationToken.None;

            //AwaitOperation.Sequential is FIFO
            //Recursion will conflict the rule
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
    }
}