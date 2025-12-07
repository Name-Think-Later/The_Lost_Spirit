using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Skill.Manifest.Event;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    public class ManifestEntity : SkillEntity, IEntity<ManifestID>
    {
        readonly IEventBus _eventBus;
        readonly Manifest  _manifest;

        public new ManifestID ID { get; }

        public ManifestEntity(ManifestID id, ManifestConfig config) : base(id) {
            ID = id;

            _eventBus = AppScope.EventBus;
            _manifest = new Manifest(config);
        }

        public override async UniTask Activate(FormulaPayload payload) {
            payloadCache.Add(payload.seq, payload);

            var manifestActivated = new AsyncManifestActivatedEvent(payload);
            await _eventBus.PublishAsync(manifestActivated);

            payloadCache.Remove(payload.seq);
        }

        public void Affect(PayloadSeq payloadSeq) { }
    }
}