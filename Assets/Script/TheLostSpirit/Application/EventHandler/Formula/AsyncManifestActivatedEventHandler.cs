using System;
using Cysharp.Threading.Tasks;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Domain.Skill.Manifest.Event;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class AsyncManifestActivatedEventHandler : AsyncDomainEventHandler<AsyncManifestActivatedEvent>
    {
        readonly AnchorRepository _anchorRepository;

        public AsyncManifestActivatedEventHandler(AnchorRepository anchorRepository) {
            _anchorRepository = anchorRepository;
        }

        protected override async UniTask Handle(AsyncManifestActivatedEvent domainEvent) {
            var payload = domainEvent.Payload;
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            if (!payload.isLastChild) return;

            payload.LastAnchors.ForEach(anchorID => {
                var anchorEntity = _anchorRepository.TakeByID(anchorID);
                anchorEntity.Destroy();
            });
        }
    }
}