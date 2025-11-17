using Cysharp.Threading.Tasks;
using R3;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.EventDriven;

namespace TheLostSpirit.Application.EventHandler
{
    public abstract class AwaitableEventHandler<TEvent> where TEvent : IAwaitableDomainEvent
    {
        protected AwaitableEventHandler() {
            AppScope
                .EventBus
                .ObservableEvent<TEvent>()
                .Subscribe(async domainEvent => {
                    await Handle(domainEvent);
                    domainEvent.Completion.TrySetResult();
                });
        }

        protected abstract UniTask Handle(TEvent domainEvent);
    }
}