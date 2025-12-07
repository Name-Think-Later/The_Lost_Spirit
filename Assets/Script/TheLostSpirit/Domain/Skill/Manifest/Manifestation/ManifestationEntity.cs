using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill.Manifest.Event
{
    public class ManifestationEntity : IEntity<ManifestationID>
    {
        readonly IManifestationMono _manifestationMono;

        public ManifestationID ID { get; }

        public ManifestationEntity(ManifestationID id, IManifestationMono manifestationMono) {
            ID = id;

            _manifestationMono = manifestationMono;
            _manifestationMono.Initialize(ID);
        }
    }
}