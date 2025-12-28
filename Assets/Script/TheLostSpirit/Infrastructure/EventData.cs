using System;
using System.Collections.Generic;
using System.Linq;
using Script.TheLostSpirit.Infrastructure.Editor;
using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;
using UnityEngine;

namespace TheLostSpirit.Infrastructure
{
    [Serializable]
    public class EventData
    {
        [ListDrawerSettings(ShowIndexLabels = true, DraggableItems = false), ListItemSelector(nameof(UpdateSelection))]
        public List<CombatStep> combatSteps = new List<CombatStep>();

        [HideInInspector]
        public int selectedIndex = -1;

        public bool CantInspectDuration =>
            !combatSteps.Any() || selectedIndex < 0 || selectedIndex >= combatSteps.Count;

        public void UpdateSelection(int index) {
            selectedIndex = index;
        }

        public void ResetSelection() {
            selectedIndex = -1;
        }
    }
}
