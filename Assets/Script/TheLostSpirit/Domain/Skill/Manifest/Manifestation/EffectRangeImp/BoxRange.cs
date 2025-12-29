using DCFApixels;
using UnityEngine;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectRangeImp
{
    public class BoxRange : EffectRange
    {
        [SerializeField]
        Vector2 _elementalSize;

        Vector2 GetSize(ManifestationSubject subject) => subject.Transform.localScale * _elementalSize;

        public override Collider2D[] Overlap(ManifestationSubject subject) {
            var position = GetPosition(subject);
            var size     = GetSize(subject);
            var result   = Physics2D.OverlapBoxAll(position, size, 0, layerMask);
#if UNITY_EDITOR
            DebugDraw(subject.Transform);
#endif
            return result;
        }

#if UNITY_EDITOR
        public override void DebugDraw(Transform previewSubject) {
            var position = (Vector2)previewSubject.position + _offset;
            var size     = previewSubject.localScale * _elementalSize;
            
            DebugX
                .Draw(debugColor)
                .Cube(position, Quaternion.identity, size);
        }
#endif
    }
}