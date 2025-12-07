using Cysharp.Threading.Tasks;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Domain.Player;
using TheLostSpirit.Domain.Skill.Core.Event;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class AsyncCoreActivatedEventHandler : AsyncDomainEventHandler<AsyncCoreActivatedEvent>
    {
        readonly CreateAnchorUseCase _createAnchorUseCase;
        readonly AnchorRepository    _anchorRepository;

        public AsyncCoreActivatedEventHandler(
            CreateAnchorUseCase createAnchorUseCase,
            AnchorRepository    anchorRepository
        ) {
            _createAnchorUseCase = createAnchorUseCase;
            _anchorRepository    = anchorRepository;
        }

        protected override UniTask Handle(AsyncCoreActivatedEvent domainEvent) {
            var payload = domainEvent.Payload;

            var output       = _createAnchorUseCase.Execute();
            var anchorEntity = _anchorRepository.GetByID(output.AnchorID);

            var playerPosition = PlayerEntity.Get().ReadOnlyTransform.Position;
            anchorEntity.SetPosition(playerPosition);

            payload.NewAnchors.Add(output.AnchorID);

            return UniTask.CompletedTask;
        }
    }
}