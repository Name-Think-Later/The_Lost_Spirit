using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Identity.EntityID;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public interface IManifestationMono : IEntityMono<ManifestationID>
    {
        public void Initialize(
            ManifestationID id,
            FrameActions    combatAction,
            FormulaPayload  payload
        );

        public void DoFrameActions(int index);
    }
}