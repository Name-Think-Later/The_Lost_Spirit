using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Formula.Node.Event;
using TheLostSpirit.Domain.Skill.Core.Event;
using TheLostSpirit.Domain.Skill.Manifest.Event;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class AsyncFormulaActivatedSaga
        : IAsyncDomainEventHandler<AsyncVisitedNodeEvent>,
          IAsyncDomainEventHandler<AsyncCoreActivatedEvent>,
          IAsyncDomainEventHandler<AsyncManifestActivatedEvent>
    {
        public AsyncFormulaActivatedSaga() {
            var token = CancellationToken.None;

            AppScope
                .EventBus
                .ObservableEvent<AsyncVisitedNodeEvent>()
                .SubscribeAwait(
                    async (domainEvent, token) => {
                        await Handle(domainEvent);
                        domainEvent.Complete();
                    },
                    AwaitOperation.Parallel
                );

            AppScope
                .EventBus
                .ObservableEvent<AsyncCoreActivatedEvent>()
                .SubscribeAwait(
                    async (domainEvent, token) => {
                        await Handle(domainEvent);
                        domainEvent.Complete();
                    },
                    AwaitOperation.Parallel
                );

            AppScope
                .EventBus
                .ObservableEvent<AsyncManifestActivatedEvent>()
                .SubscribeAwait(
                    async (domainEvent, token) => {
                        await Handle(domainEvent);
                        domainEvent.Complete();
                    },
                    AwaitOperation.Parallel
                );
        }

        public UniTask Handle(AsyncVisitedNodeEvent domainEvent) {
            throw new System.NotImplementedException();
        }

        public UniTask Handle(AsyncCoreActivatedEvent domainEvent) {
            throw new System.NotImplementedException();
        }

        public UniTask Handle(AsyncManifestActivatedEvent domainEvent) {
            throw new System.NotImplementedException();
        }
    }
}