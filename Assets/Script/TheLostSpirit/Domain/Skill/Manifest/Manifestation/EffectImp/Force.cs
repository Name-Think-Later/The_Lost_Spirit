using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Formula;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectImp
{
    public class Force : Effect
    {
        [SerializeField, EnumToggleButtons]
        DirectionMode _directionMode;

        [SerializeField]
        Vector2 _direction;

        [SerializeField]
        float _elementalStrength;

        public override void Apply(IEntityMono target, ManifestationSubject subject) {
            var relativeDirection = _directionMode switch {
                DirectionMode.Absolute => _direction,
                DirectionMode.Relative => Vector2.zero,
                _                      => Vector2.zero
            };
        }
    }


    public enum DirectionMode
    {
        Absolute,
        Relative
    }
}