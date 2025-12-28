using DCFApixels;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectRangeImp
{
    public class BoxRange : EffectRange
    {
        [SerializeField]
        Rect _rect;

        public override Collider2D[] Overlap() {
            var position = (Vector2)transform.position + _rect.position;
            var result   = Physics2D.OverlapBoxAll(position, _rect.size, 0, layerMask);
#if UNITY_EDITOR
            DebugDraw(transform.position);
#endif
            return result;
        }

#if UNITY_EDITOR
        public override void DebugDraw(Vector2 pivot) {
            var position = pivot + _rect.position;

            DebugX
                .Draw(new Color(1, 0.2f, 0.2f, 0.5f))
                .Cube(position, Quaternion.identity, _rect.size);
        }
#endif
    }
}