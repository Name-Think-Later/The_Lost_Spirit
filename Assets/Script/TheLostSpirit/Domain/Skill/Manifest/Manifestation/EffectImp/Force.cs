using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Component;
using TheLostSpirit.Domain.Formula;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectImp
{
    public class Force : Effect
    {
        [SerializeField, EnumToggleButtons]
        DirectionMode _directionMode;

        [SerializeField, Range(0, 360)]
        float _angle;

        [SerializeField]
        float _elementalStrength;

        public override void Apply(IEntityMono target, ManifestationSubject subject) {
            if (target is not IForceApplyingComponent forceApplying) {
                return;
            }


            var finalAngle = _directionMode switch {
                DirectionMode.Absolute => _angle,
                DirectionMode.Relative => subject.Transform.eulerAngles.z + _angle,
                _                      => 0
            };

            var rad       = finalAngle * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            Debug.Log(subject.Transform.eulerAngles);
            Debug.Log(direction);

            var force = direction * _elementalStrength;

            forceApplying.ApplyForce(force);
        }
    }


    public enum DirectionMode
    {
        Absolute,
        Relative
    }
}