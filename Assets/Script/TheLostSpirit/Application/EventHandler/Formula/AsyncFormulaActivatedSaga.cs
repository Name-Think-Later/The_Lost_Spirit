using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MoreLinq;
using R3;
using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Formula.Node;
using TheLostSpirit.Domain.Formula.Node.Event;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Skill.Core.Event;
using TheLostSpirit.Domain.Skill.Manifest.Event;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Identity.SpecificationID;
using UnityEngine;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class AsyncFormulaActivatedSaga
        : IAsyncDomainEventHandler<AsyncVisitedNodeEvent>,
          IAsyncDomainEventHandler<AsyncCoreActivatedEvent>,
          IAsyncDomainEventHandler<AsyncManifestActivatedEvent>
    {
        readonly NodeRepository                _nodeRepository;
        readonly AnchorRepository              _anchorRepository;
        readonly ManifestationRepository       _manifestationRepository;
        readonly ManifestationViewModelStore   _manifestationViewModelStore;
        readonly IManifestationInstanceFactory _manifestationInstanceFactory;

        readonly ActiveSkillUseCase  _activeSkillUseCase;
        readonly CreateAnchorUseCase _createAnchorUseCase;

        readonly IDisposable                      _disposable;
        readonly Dictionary<Guid, FormulaPayload> _payloads = new();

        public AsyncFormulaActivatedSaga(
            NodeRepository                nodeRepository,
            AnchorRepository              anchorRepository,
            ManifestationRepository       manifestationRepository,
            ManifestationViewModelStore   manifestationViewModelStore,
            IManifestationInstanceFactory manifestationInstanceFactory,
            ActiveSkillUseCase            activeSkillUseCase,
            CreateAnchorUseCase           createAnchorUseCase
        ) {
            _nodeRepository               = nodeRepository;
            _anchorRepository             = anchorRepository;
            _manifestationRepository      = manifestationRepository;
            _manifestationViewModelStore  = manifestationViewModelStore;
            _manifestationInstanceFactory = manifestationInstanceFactory;
            _activeSkillUseCase           = activeSkillUseCase;
            _createAnchorUseCase          = createAnchorUseCase;

            var disposableBuilder = new DisposableBuilder();

            AppScope
                .EventBus
                .ObservableEvent<AsyncVisitedNodeEvent>()
                .SubscribeAwait(
                    async (domainEvent, token) => {
                        await Handle(domainEvent);
                        domainEvent.Complete();
                    },
                    AwaitOperation.Parallel
                )
                .AddTo(ref disposableBuilder);

            AppScope
                .EventBus
                .ObservableEvent<AsyncCoreActivatedEvent>()
                .SubscribeAwait(
                    async (domainEvent, token) => {
                        await Handle(domainEvent);
                        domainEvent.Complete();
                    },
                    AwaitOperation.Parallel
                )
                .AddTo(ref disposableBuilder);

            AppScope
                .EventBus
                .ObservableEvent<AsyncManifestActivatedEvent>()
                .SubscribeAwait(
                    async (domainEvent, token) => {
                        await Handle(domainEvent);
                        domainEvent.Complete();
                    },
                    AwaitOperation.Parallel
                )
                .AddTo(ref disposableBuilder);

            _disposable = disposableBuilder.Build();
        }

        public async UniTask Handle(AsyncVisitedNodeEvent domainEvent) {
            var nodeID       = domainEvent.NodeID;
            var oldSequentID = domainEvent.SequentID;
            var isLastChild  = domainEvent.IsLastChild;

            var nodeEntity = _nodeRepository.GetByID(nodeID);

            Debug.Log(nodeID.Index);

            var gotPayload = _payloads.TryGetValue(oldSequentID, out var oldPayload);
            var newPayload = gotPayload ? oldPayload.Clone() : new FormulaPayload();
            _payloads.Add(newPayload.SequentID, newPayload);

            var activeSkillInput = new ActiveSkillUseCase.Input(nodeEntity.SkillID, newPayload);
            await _activeSkillUseCase.Execute(activeSkillInput);

            newPayload.AddDebugRoute(nodeID);

            var isLeafNode = !await nodeEntity.MoveNext(newPayload, TraversalPolicy.Parallel);

            if (isLeafNode) DestroyAnchors(newPayload.CandidateAnchors);

            var shouldDestroyAnchors = (isLastChild && newPayload.AnchorConsumed) || (!isLastChild && isLeafNode);
            if (shouldDestroyAnchors) DestroyAnchors(newPayload.Anchors);

            _payloads.Remove(newPayload.SequentID);
        }

        void DestroyAnchors(List<AnchorID> anchors) {
            foreach (var anchorID in anchors) {
                var anchorEntity = _anchorRepository.TakeByID(anchorID);
                anchorEntity.Destroy();
            }

            anchors.Clear();
        }

        public UniTask Handle(AsyncCoreActivatedEvent domainEvent) {
            var sequentID = domainEvent.SequentID;

            var playerPosition = PlayerEntity.Get().ReadOnlyTransform.Position;
            var input          = new CreateAnchorUseCase.Input(playerPosition, Vector2.zero);
            var output         = _createAnchorUseCase.Execute(input);
            var anchorEntity   = _anchorRepository.GetByID(output.AnchorID);

            var payload = _payloads[sequentID];
            payload.Anchors.Add(anchorEntity.ID);
            anchorEntity.SetActive(true);

            return UniTask.CompletedTask;
        }

        public async UniTask Handle(AsyncManifestActivatedEvent domainEvent) {
            Debug.Log("Manifestation");
            var specificationID = domainEvent.ManifestationSpecificationID;
            var sequentID       = domainEvent.SequentID;

            var payload = _payloads[sequentID];

            var manifestationsFinish = new List<UniTask>();
            foreach (var anchorID in payload.Anchors) {
                var manifestationEntity = CreateManifestationAndRegistry(anchorID, specificationID, payload);
                manifestationsFinish.Add(manifestationEntity.IsFinish.WaitAsync());

                var manifestationPosition = manifestationEntity.ReadOnlyTransform.Position;
                var input                 = new CreateAnchorUseCase.Input(manifestationPosition, Vector2.zero);
                var output                = _createAnchorUseCase.Execute(input);
                var anchorEntity          = _anchorRepository.GetByID(output.AnchorID);

                payload.CandidateAnchors.Add(output.AnchorID);
                anchorEntity.SetActive(true);
            }

            await UniTask.WhenAll(manifestationsFinish);
        }

        ManifestationEntity CreateManifestationAndRegistry(
            AnchorID                     anchorID,
            ManifestationSpecificationID specificationID,
            FormulaPayload               payload
        ) {
            var anchorEntity = _anchorRepository.GetByID(anchorID);
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

        public void Dispose() {
            _disposable.Dispose();
        }
    }
}