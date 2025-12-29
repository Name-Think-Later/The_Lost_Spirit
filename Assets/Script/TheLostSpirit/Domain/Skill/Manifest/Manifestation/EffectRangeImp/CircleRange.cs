using DCFApixels;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectRangeImp
{
    public class CircleRange : EffectRange
    {
        [SerializeField]
        float _elementalRadius;

        float GetRadius(ManifestationSubject subject) =>
            Mathf.Max(subject.Transform.localScale.x, subject.Transform.localScale.y) * _elementalRadius;

        public override Collider2D[] Overlap(ManifestationSubject subject) {
            var position = GetPosition(subject);
            var radius   = GetRadius(subject);
            var result   = Physics2D.OverlapCircleAll(position, radius, layerMask);
#if UNITY_EDITOR
            DebugDraw(subject.Transform);
#endif
            return result;
        }

#if UNITY_EDITOR
        public override void DebugDraw(Transform previewSubject) {
            var position = (Vector2)previewSubject.position + offset;
            var radius   = Mathf.Max(previewSubject.localScale.x, previewSubject.localScale.y) * _elementalRadius;

            DebugX
                .Draw(debugColor)
                .Circle(position, Quaternion.identity, radius);
        }
#endif
    }
}