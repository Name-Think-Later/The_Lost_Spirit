using Cysharp.Threading.Tasks;
using R3;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Port.EventBus;

namespace TheLostSpirit.Application.EventHandler
{
    public abstract class AsyncDomainEventHandler<TEvent> where TEvent : IAsyncDomainEvent
    {
        protected AsyncDomainEventHandler() {
            AppScope
                .EventBus
                .ObservableEvent<TEvent>()
                .Subscribe(async domainEvent => {
                    await Handle(domainEvent);
                    domainEvent.Complete();
                });
        }

        protected abstract UniTask Handle(TEvent domainEvent);
    }
}