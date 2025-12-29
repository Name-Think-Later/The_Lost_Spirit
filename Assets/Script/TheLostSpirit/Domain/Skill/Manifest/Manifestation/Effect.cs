using System;
using TheLostSpirit.Domain.Formula;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    [Serializable]
    public abstract class Effect
    {
        public abstract void Apply(IEntityMono target, FormulaPayload payload);
    }
}