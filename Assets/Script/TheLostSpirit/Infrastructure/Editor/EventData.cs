using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TheLostSpirit.Domain.Skill.Manifest.Manifestation;

namespace TheLostSpirit.Infrastructure.Editor
{
    [Serializable]
    public class EventData
    {
        [ListDrawerSettings(ShowIndexLabels = true, DraggableItems = false)]
        [ListItemSelector(nameof(UpdateSelection))]
        public List<CombatStep> combatActions = new List<CombatStep>();


        public int SelectedIndex { get; private set; }

        public bool CantInspectDuration =>
            !combatActions.Any() || SelectedIndex < 0 || SelectedIndex >= combatActions.Count;

        void UpdateSelection(int index) => SelectedIndex = index;
        public void ResetSelection() => SelectedIndex = -1;
    }
}