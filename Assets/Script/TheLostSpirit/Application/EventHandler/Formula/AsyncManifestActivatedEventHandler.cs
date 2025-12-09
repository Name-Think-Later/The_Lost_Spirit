using System;
using Cysharp.Threading.Tasks;
using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Skill.Manifest.Event;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class AsyncManifestActivatedEventHandler : AsyncDomainEventHandler<AsyncManifestActivatedEvent>
    {
        readonly AnchorRepository              _anchorRepository;
        readonly IManifestationInstanceFactory _manifestationInstanceFactory;
        readonly ManifestationRepository       _manifestationRepository;
        readonly ManifestationViewModelStore   _manifestationViewModelStore;

        public AsyncManifestActivatedEventHandler(
            AnchorRepository              anchorRepository,
            ManifestationRepository       manifestationRepository,
            ManifestationViewModelStore   manifestationViewModelStore,
            IManifestationInstanceFactory manifestationInstanceFactory
        ) {
            _anchorRepository             = anchorRepository;
            _manifestationRepository      = manifestationRepository;
            _manifestationViewModelStore  = manifestationViewModelStore;
            _manifestationInstanceFactory = manifestationInstanceFactory;
        }

        protected override async UniTask Handle(AsyncManifestActivatedEvent domainEvent) {
            var payload               = domainEvent.Payload;
            var manifestationConfigID = domainEvent.ManifestationConfigID;

            var instanceContext = _manifestationInstanceFactory.Create(manifestationConfigID, payload);
            _manifestationRepository.Save(instanceContext.Entity);
            _manifestationViewModelStore.Save(instanceContext.ViewModelReference);

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            if (!payload.isLastChild) {
                return;
            }

            payload.LastAnchors.ForEach(anchorID => {
                var anchorEntity = _anchorRepository.TakeByID(anchorID);
                anchorEntity.Destroy();
            });
        }
    }
}