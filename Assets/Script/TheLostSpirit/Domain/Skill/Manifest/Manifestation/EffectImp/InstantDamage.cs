using TheLostSpirit.Domain.Formula;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectImp
{
    public class InstantDamage : Effect
    {
        [SerializeField]
        float _elementalDamage;

        public override void Apply(IEntityMono target, ManifestationSubject subject) {
            if (target is not IDamageableComponent damageable) {
                return;
            }

            var finalDamage = _elementalDamage + subject.Payload.testFactor;
            damageable.DealDamage(finalDamage);
        }
    }
}