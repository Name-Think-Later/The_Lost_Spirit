using TheLostSpirit.Domain.Repository;
using TheLostSpirit.Domain.Skill.Weave;

namespace TheLostSpirit.Application.EventHandler.Formula
{
    public class WeaveActivatedEventHandler : DomainEventHandler<WeaveActivatedEvent>
    {
        readonly AnchorRepository _anchorRepository;

        public WeaveActivatedEventHandler(AnchorRepository anchorRepository) {
            _anchorRepository = anchorRepository;
        }

        public override void Handle(WeaveActivatedEvent domainEvent) {
            var payload = domainEvent.Payload;

            foreach (var anchorID in payload.Anchors) {
                var anchorEntity = _anchorRepository.GetByID(anchorID);
                anchorEntity.SetActive(true);
            }
        }
    }
}