using DCFApixels;
using TheLostSpirit.Domain.Formula;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectRangeImp
{
    public class BoxRange : EffectRange
    {
        [SerializeField]
        Vector2 _elementalSize;

        Vector2 Size => pivot.localScale * _elementalSize;

        public override Collider2D[] Overlap() {
            var result = Physics2D.OverlapBoxAll(Position, Size, 0, layerMask);
#if UNITY_EDITOR
            DebugDraw();
#endif
            return result;
        }

#if UNITY_EDITOR
        public override void DebugDraw() {
            DebugX
                .Draw(debugColor)
                .Cube(Position, Quaternion.identity, Size);
        }
#endif
    }
}