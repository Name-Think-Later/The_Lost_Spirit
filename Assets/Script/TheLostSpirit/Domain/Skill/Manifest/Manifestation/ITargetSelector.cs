using System.Collections.Generic;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public interface ITargetSelector
    {
        public IEnumerable<IEntityMono> GetTargets(ManifestationSubject subject);

#if UNITY_EDITOR
        public void DebugDrawRange(Transform previewSubject);
#endif
    }
}