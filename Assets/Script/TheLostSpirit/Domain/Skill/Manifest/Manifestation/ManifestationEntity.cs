using Cysharp.Threading.Tasks;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public class ManifestationEntity : IEntity<ManifestationID>
    {
        readonly Manifestation      _manifestation;
        readonly IManifestationMono _manifestationMono;
        public ManifestationID ID { get; }

        public AsyncReactiveProperty<bool> IsFinish => _manifestation.isFinish;

        public ManifestationEntity(
            ManifestationID     id,
            ManifestationConfig config,
            IManifestationMono  manifestationMono
        ) {
            ID                 = id;
            _manifestation     = new Manifestation(config);
            _manifestationMono = manifestationMono;
            _manifestationMono.Initialize(ID, config.FrameActions);
        }

        public void DoFrameActions(int index) {
            _manifestationMono.DoFrameActions(index);
        }

        public void Finish() {
            _manifestation.isFinish.Value = true;
        }

        public void Destroy() {
            _manifestationMono.Destroy();
        }
    }
}