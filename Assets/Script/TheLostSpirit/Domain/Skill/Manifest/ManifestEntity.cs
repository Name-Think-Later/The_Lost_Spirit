using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    public class ManifestEntity : IEntity<ManifestID>
    {
        readonly Manifest _manifest;

        public ManifestID ID { get; }

        public ManifestEntity(ManifestID id) {
            ID = id;

            _manifest = new Manifest();
        }
    }
}