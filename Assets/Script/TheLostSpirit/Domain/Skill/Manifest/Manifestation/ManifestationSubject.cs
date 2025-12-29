using TheLostSpirit.Domain.Formula;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    /// <summary>
    /// Represents the subject (individual entity) that manifests combat effects.
    /// Encapsulates runtime dependencies for skill execution.
    /// </summary>
    public class ManifestationSubject
    {
        public Transform Transform { get; }
        public FormulaPayload Payload { get; }

        public ManifestationSubject(Transform transform, FormulaPayload payload) {
            Transform = transform;
            Payload   = payload;
        }
    }
}