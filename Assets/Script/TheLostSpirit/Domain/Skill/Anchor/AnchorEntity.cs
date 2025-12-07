using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Anchor
{
    public class AnchorEntity : IEntity<AnchorID>
    {
        readonly IEventBus   _eventBus;
        readonly IAnchorMono _anchorMono;
        public AnchorID ID { get; }
        public IReadOnlyTransform ReadOnlyTransform => _anchorMono.ReadOnlyTransform;

        public AnchorEntity(AnchorID id, IAnchorMono anchorMono) {
            ID        = id;
            _eventBus = AppScope.EventBus;

            _anchorMono = anchorMono;

            anchorMono.Initialize(id);
        }

        public void SetPosition(Vector2 position) {
            _anchorMono.SetPosition(position);
        }

        public void Destroy() {
            _anchorMono.Destroy();
        }
    }
}