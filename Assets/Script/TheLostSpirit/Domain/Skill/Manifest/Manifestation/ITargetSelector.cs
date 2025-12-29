using System.Collections.Generic;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public interface ITargetSelector
    {
        public IEnumerable<IEntityMono> GetTargets();
        public void SetOwner(Transform owner); // Needed for editor callback

#if UNITY_EDITOR
        public void DebugDrawRange();
#endif
    }
}