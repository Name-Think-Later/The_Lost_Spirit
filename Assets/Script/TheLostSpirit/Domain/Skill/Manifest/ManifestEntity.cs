using Cysharp.Threading.Tasks;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Port.EventBus;
using TheLostSpirit.Domain.Skill.Manifest.Event;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    public class ManifestEntity : SkillEntity, IEntity<ManifestID>
    {
        readonly IEventBus _eventBus;
        readonly Manifest  _manifest;

        public ManifestEntity(ManifestID id, ManifestConfig config) : base(id) {
            ID = id;

            _eventBus = AppScope.EventBus;
            _manifest = new Manifest(config);
        }

        public new ManifestID ID { get; }

        public override async UniTask Activate(FormulaPayload payload) {
            var manifestActivated = new AsyncManifestActivatedEvent(_manifest.Manifestation, payload);
            await _eventBus.PublishAsync(manifestActivated);
        }
    }
}