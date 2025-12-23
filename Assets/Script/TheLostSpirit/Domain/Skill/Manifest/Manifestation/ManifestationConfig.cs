using System;
using Sirenix.Serialization;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    [Serializable]
    public class ManifestationConfig
    {
        [OdinSerialize, HideInInspector]
        public FrameActions FrameActions { get; private set; }


#if UNITY_EDITOR
        public void SetCombatActions(FrameActions frameActions) {
            FrameActions = frameActions;
        }
#endif
    }
}