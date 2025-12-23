using TheLostSpirit.Identity.SpecificationID;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    public class Manifest
    {
        readonly ManifestConfig _config;

        public Manifest(ManifestConfig config) {
            _config = config;
        }

        public ManifestationSpecificationID Manifestation => _config.ManifestationSpecificationID;
    }
}