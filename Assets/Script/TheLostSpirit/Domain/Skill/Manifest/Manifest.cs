using TheLostSpirit.Identity.ConfigID;

namespace TheLostSpirit.Domain.Skill.Manifest
{
    public class Manifest
    {
        readonly ManifestConfig _config;

        public Manifest(ManifestConfig config) {
            _config = config;
        }

        public ManifestationConfigID Manifestation => _config.ManifestationConfigID;
    }
}