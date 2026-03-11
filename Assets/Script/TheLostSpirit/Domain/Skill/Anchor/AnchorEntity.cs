using System;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Port.ReadOnly;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Anchor
{
    public class AnchorEntity : IEntity<AnchorID>
    {
        readonly IAnchorMono _anchorMono;
        readonly IEventBus   _eventBus;

        public Guid FormulaStreamID { get; }

        public AnchorEntity(AnchorID id, IAnchorMono anchorMono, Guid formulaStreamID) {
            ID          = id;
            _eventBus   = AppScope.EventBus;
            _anchorMono = anchorMono;
            anchorMono.Initialize(id);
            FormulaStreamID = formulaStreamID;
        }

        public IReadOnlyTransform ReadOnlyTransform => _anchorMono.ReadOnlyTransform;
        public AnchorID ID { get; }

        public void SetPosition(Vector2 position) {
            _anchorMono.SetPosition(position);
        }

        public void SetRotation(Vector2 rotation) {
            _anchorMono.SetRotation(rotation);
        }

        public void SetActive(bool active) {
            _anchorMono.SetActive(active);
        }

        public void Destroy() {
            _anchorMono.Destroy();
        }
    }
}