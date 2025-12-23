using System;
namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    [Serializable]
    public abstract class Effect
    {
        public abstract void Apply(IEntityMono entityMono);
    }
}