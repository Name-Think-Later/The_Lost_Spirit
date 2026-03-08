using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MoreLinq;
using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Skill.Anchor;
using TheLostSpirit.Domain.Skill.Manifest.Event;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Identity.SpecificationID;
using UnityEngine;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class AsyncManifestActivatedEventHandler : AsyncDomainEventHandler<AsyncManifestActivatedEvent>
    {
        readonly AnchorRepository              _anchorRepository;
        readonly ManifestationRepository       _manifestationRepository;
        readonly ManifestationViewModelStore   _manifestationViewModelStore;
        readonly IManifestationInstanceFactory _manifestationInstanceFactory;

        readonly CreateAnchorUseCase _createAnchorUseCase;

        public AsyncManifestActivatedEventHandler(
            AnchorRepository              anchorRepository,
            ManifestationRepository       manifestationRepository,
            ManifestationViewModelStore   manifestationViewModelStore,
            IManifestationInstanceFactory manifestationInstanceFactory,
            CreateAnchorUseCase           createAnchorUseCase
        ) {
            _anchorRepository             = anchorRepository;
            _manifestationRepository      = manifestationRepository;
            _manifestationViewModelStore  = manifestationViewModelStore;
            _manifestationInstanceFactory = manifestationInstanceFactory;
            _createAnchorUseCase          = createAnchorUseCase;
        }

        protected override async UniTask Handle(AsyncManifestActivatedEvent domainEvent) {
            var payload         = domainEvent.Payload;
            var specificationID = domainEvent.ManifestationSpecificationID;

            var manifestationsFinish = new List<UniTask>();
            foreach (var lastAnchorID in payload.LastAnchors) {
                var manifestationEntity = CreateManifestationAndRegistry(lastAnchorID, specificationID, payload);
                manifestationsFinish.Add(manifestationEntity.IsFinish.WaitAsync());

                var manifestationPosition = manifestationEntity.ReadOnlyTransform.Position;
                var input                 = new CreateAnchorUseCase.Input(manifestationPosition, Vector2.zero);
                var output                = _createAnchorUseCase.Execute(input);

                payload.NewAnchors.Add(output.AnchorID);
            }

            await UniTask.WhenAll(manifestationsFinish);

            if (payload.IsLastChild) ClearLastAnchor(payload);
        }

        ManifestationEntity CreateManifestationAndRegistry(
            AnchorID                     lastAnchorID,
            ManifestationSpecificationID specificationID,
            FormulaPayload               payload
        ) {
            var anchorEntity = _anchorRepository.GetByID(lastAnchorID);
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
        }

        void ClearLastAnchor(FormulaPayload payload) {
            foreach (var anchorID in payload.LastAnchors) {
                var anchorEntity = _anchorRepository.TakeByID(anchorID);
                anchorEntity.Destroy();
            }

            payload.LastAnchors.Clear();
        }
    }
}