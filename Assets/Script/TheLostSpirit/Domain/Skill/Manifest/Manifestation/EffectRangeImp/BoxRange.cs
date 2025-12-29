using DCFApixels;
using UnityEngine;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation.EffectRangeImp
{
    public class BoxRange : EffectRange
    {
        [SerializeField, Range(0, 360)]
        float _elementalRotation;

        [SerializeField]
        Vector2 _elementalSize;

        Vector2 GetSize(ManifestationSubject subject) => subject.Transform.localScale * _elementalSize;

        float GetRotation(ManifestationSubject subject) => subject.Transform.eulerAngles.z + _elementalRotation;

        public override Collider2D[] Overlap(ManifestationSubject subject) {
            var position = GetPosition(subject);
            var size     = GetSize(subject);
            var rotation = GetRotation(subject);
            var result   = Physics2D.OverlapBoxAll(position, size, rotation, layerMask);
#if UNITY_EDITOR
            DebugDraw(subject.Transform);
#endif
            return result;
        }

#if UNITY_EDITOR
        public override void DebugDraw(Transform previewSubject) {
            var position   = (Vector2)previewSubject.position + offset;
            var size       = previewSubject.localScale * _elementalSize;
            var rotation   = previewSubject.eulerAngles.z + _elementalRotation;
            var quaternion = Quaternion.AngleAxis(rotation, Vector3.forward);

            DebugX
                .Draw(debugColor)
                .Cube(position, quaternion, size);
        }
#endif
    }
}