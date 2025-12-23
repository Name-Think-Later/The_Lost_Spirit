using System.Collections.Generic;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    public interface ITargetSelector
    {
        public void Initialize(Transform transform);

        public IEnumerable<IEntityMono> GetTargets();
    }
}