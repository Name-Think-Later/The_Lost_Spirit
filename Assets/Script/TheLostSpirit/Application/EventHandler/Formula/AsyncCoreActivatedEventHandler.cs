using Cysharp.Threading.Tasks;
using TheLostSpirit.Application.UseCase;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Repository;
using TheLostSpirit.Domain.Skill.Core.Event;
using UnityEngine;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class AsyncCoreActivatedEventHandler : AsyncDomainEventHandler<AsyncCoreActivatedEvent>
    {
        readonly AnchorRepository    _anchorRepository;
        readonly CreateAnchorUseCase _createAnchorUseCase;

        public AsyncCoreActivatedEventHandler(
            CreateAnchorUseCase createAnchorUseCase,
            AnchorRepository    anchorRepository
        ) {
            _createAnchorUseCase = createAnchorUseCase;
            _anchorRepository    = anchorRepository;
        }

        public override UniTask Handle(AsyncCoreActivatedEvent domainEvent) {
            var payload = domainEvent.Payload;

            var playerPosition = PlayerEntity.Get().ReadOnlyTransform.Position;
            var input          = new CreateAnchorUseCase.Input(playerPosition, Vector2.zero, payload.FormulaStreamID);
            var output         = _createAnchorUseCase.Execute(input);
            var anchorEntity   = _anchorRepository.GetByID(output.AnchorID);

            payload.Anchors.Add(anchorEntity.ID);
            anchorEntity.SetActive(true);

            return UniTask.CompletedTask;
        }
    }
}