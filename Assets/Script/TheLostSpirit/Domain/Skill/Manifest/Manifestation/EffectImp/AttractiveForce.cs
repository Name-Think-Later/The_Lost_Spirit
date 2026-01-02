using System;
using TheLostSpirit.Domain.Component;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectImp
{
    public class AttractiveForce : Effect
    {
        [SerializeField]
        bool _useFalloff;

        [SerializeField]
        float _elementalStrength;

        public override void Apply(IEntityMono target, ManifestationSubject subject) {
            if (target is not IForceApplyingComponent forceApplying) {
                return;
            }

            var vector   = (Vector2)(subject.Transform.position - target.ReadOnlyTransform.Position);
            var distance = vector.magnitude;

            if (Mathf.Approximately(0, distance)) return;

            var direction = vector.normalized;
            var forceMagnitude =
                _useFalloff ?
                    _elementalStrength / (distance * distance) :
                    _elementalStrength;

            forceApplying.ApplyForce(direction * forceMagnitude);
        }
    }
}