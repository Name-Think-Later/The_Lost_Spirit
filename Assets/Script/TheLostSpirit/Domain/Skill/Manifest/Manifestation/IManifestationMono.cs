using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public interface IManifestationMono : IEntityMono<ManifestationID>
    {
        public void Initialize(
            ManifestationID id,
            FrameActions    combatAction
        );

        public void DoFrameActions(int index);
    }
}