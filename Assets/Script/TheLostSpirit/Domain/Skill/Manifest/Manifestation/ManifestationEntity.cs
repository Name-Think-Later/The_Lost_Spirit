using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public class ManifestationEntity : IEntity<ManifestationID>
    {
        readonly IManifestationMono _manifestationMono;

        public ManifestationEntity(ManifestationID id, IManifestationMono manifestationMono) {
            ID = id;

            _manifestationMono = manifestationMono;
            _manifestationMono.Initialize(ID);
        }

        public ManifestationID ID { get; }
    }
}