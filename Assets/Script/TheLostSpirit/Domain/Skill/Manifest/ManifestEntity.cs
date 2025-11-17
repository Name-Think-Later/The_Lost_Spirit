using TheLostSpirit.Identify;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    public class ManifestEntity : SkillEntity, IEntity<ManifestID>
    {
        readonly Manifest _manifest;

        public new ManifestID ID { get; }

        public ManifestEntity(ManifestID id, ManifestConfig config) : base(id) {
            ID = id;

            _manifest = new Manifest(config);
        }

        public override void Activate() { }
    }
}