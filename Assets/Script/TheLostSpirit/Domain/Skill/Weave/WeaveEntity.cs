using System.Linq;
using Cysharp.Threading.Tasks;
using MoreLinq;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Identity.EntityID;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Weave
{
    public class WeaveEntity : SkillEntity, IEntity<WeaveID>
    {
        readonly Weave     _weave;
        readonly IEventBus _eventBus;
        public new WeaveID ID { get; }

        public WeaveEntity(WeaveID id, WeaveConfig config) : base(id) {
            ID        = id;
            _eventBus = AppScope.EventBus;
            _weave    = new Weave(config);
        }

        public override UniTask Activate(FormulaPayload payload) {
            var hasAnchor = payload.Anchors.Any();
            Debug.Log($"Weave has Anchor?: {hasAnchor} ");

            if (hasAnchor) return UniTask.CompletedTask;

            //Domain: Weaved Condition
            payload.PromoteCandidateAnchors();
            
            var weaveActivatedEvent = new WeaveActivatedEvent(payload);
            _eventBus.Publish(weaveActivatedEvent);

            return UniTask.CompletedTask;
        }
    }
}