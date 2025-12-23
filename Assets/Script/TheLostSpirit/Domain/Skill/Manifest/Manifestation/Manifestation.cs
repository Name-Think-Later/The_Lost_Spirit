using Cysharp.Threading.Tasks;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public class Manifestation
    {
        readonly ManifestationConfig _config;

        public readonly AsyncReactiveProperty<bool> isFinish = new AsyncReactiveProperty<bool>(false);

        public Manifestation(ManifestationConfig config) {
            _config = config;
        }
    }
}