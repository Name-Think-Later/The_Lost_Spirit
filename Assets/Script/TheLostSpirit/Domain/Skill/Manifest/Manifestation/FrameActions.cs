using System;
using System.Collections.Generic;

namespace TheLostSpirit.Domain.Skill.Manifest.Manifestation
{
    [Serializable]
    public class FrameActions : Dictionary<int, List<CombatStep>>
    {
        public FrameActions(Dictionary<int, List<CombatStep>> source) : base(source) { }
    }
}