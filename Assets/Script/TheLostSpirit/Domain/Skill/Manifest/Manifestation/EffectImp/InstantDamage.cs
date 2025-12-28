using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectImp
{
    public class InstantDamage : Effect
    {
        [SerializeField]
        float _elementalDamage;

        public override void Apply(IEntityMono entityMono) {
            if (entityMono is not IDamageableComponent damageable) {
                return;
            }

            damageable.DealDamage(_elementalDamage);
        }
    }
}