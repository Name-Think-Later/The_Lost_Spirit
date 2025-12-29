using DCFApixels;
using DCFApixels.DebugXCore;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectRangeImp
{
    public class CircleRange : EffectRange
    {
        [SerializeField]
        Vector2 _center;

        [SerializeField]
        float _radius;

        public override Collider2D[] Overlap() {
            var position = (Vector2)transform.position + _center;
            var result   = Physics2D.OverlapCircleAll(position, _radius, layerMask);
#if UNITY_EDITOR
            DebugDraw(transform.position);
#endif
            return result;
        }
        
#if UNITY_EDITOR
        public override void DebugDraw(Vector2 pivot) {
            var position = pivot + _center;

            DebugX
                .Draw(debugColor)
                .Circle(position, Quaternion.identity, _radius);
        }
    }
#endif
}