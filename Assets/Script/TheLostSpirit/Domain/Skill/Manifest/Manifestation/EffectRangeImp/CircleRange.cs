using DCFApixels;
using TheLostSpirit.Domain.Formula;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectRangeImp
{
    public class CircleRange : EffectRange
    {
        [SerializeField]
        float _elementalRadius;

        float Radius => Mathf.Max(pivot.localScale.x, pivot.localScale.y) * _elementalRadius;

        public override Collider2D[] Overlap() {
            var result = Physics2D.OverlapCircleAll(Position, Radius, layerMask);
#if UNITY_EDITOR
            DebugDraw();
#endif
            return result;
        }

#if UNITY_EDITOR
        public override void DebugDraw() {
            DebugX
                .Draw(debugColor)
                .Circle(Position, Quaternion.identity, Radius);
        }
    }
#endif
}