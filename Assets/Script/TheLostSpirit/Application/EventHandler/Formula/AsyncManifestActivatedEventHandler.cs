using System.Linq;
using Cysharp.Threading.Tasks;
using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Skill.Manifest.Event;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class AsyncManifestActivatedEventHandler : AsyncDomainEventHandler<AsyncManifestActivatedEvent>
    {
        readonly AnchorRepository              _anchorRepository;
        readonly ManifestationRepository       _manifestationRepository;
        readonly ManifestationViewModelStore   _manifestationViewModelStore;
        readonly IManifestationInstanceFactory _manifestationInstanceFactory;

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
            var payload         = domainEvent.Payload;
            var specificationID = domainEvent.ManifestationSpecificationID;

            var entities = CreateManyAndRegister(specificationID, payload);

            await UniTask.WhenAny(entities.Select(entity => entity.IsFinish.WaitAsync()));

            DestroyManyAndDeregister(entities);

            if (payload.IsLastChild) ClearAnchor(payload);
        }

        ManifestationEntity[] CreateManyAndRegister(
            ManifestationSpecificationID specificationID,
            FormulaPayload               payload
        ) {
            return
                payload
                    .LastAnchors
                    .Select(id => {
                        var anchorEntity = _anchorRepository.GetByID(id);
                        var instanceContext =
                            _manifestationInstanceFactory
                                .Create(
                                    specificationID,
                                    payload,
                                    anchorEntity.ReadOnlyTransform
                                );

                        var entity             = instanceContext.Entity;
                        var viewModelReference = instanceContext.ViewModelReference;

                        _manifestationRepository.Save(entity);
                        _manifestationViewModelStore.Save(viewModelReference);

                        return entity;
                    })
                    .ToArray();
        }


        void DestroyManyAndDeregister(ManifestationEntity[] entities) {
            foreach (var entity in entities) {
                var id = entity.ID;
                _manifestationRepository.Remove(id);
                _manifestationViewModelStore.Remove(id);

                entity.Destroy();
            }
        }

        void ClearAnchor(FormulaPayload payload) {
            foreach (var anchorID in payload.LastAnchors) {
                var anchorEntity = _anchorRepository.TakeByID(anchorID);
                anchorEntity.Destroy();
            }
        }
    }
}