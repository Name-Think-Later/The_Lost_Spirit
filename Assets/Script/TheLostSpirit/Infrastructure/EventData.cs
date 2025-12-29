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
        [ListDrawerSettings(
             ShowIndexLabels = true,
             DraggableItems = false,
             OnBeginListElementGUI = nameof(InjectOwner)),
         ListItemSelector(nameof(UpdateSelection))]
        public List<CombatStep> combatSteps = new List<CombatStep>();

        Transform _owner;

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

        // Public method for injecting owner from TimelineEditor
        public void SetOwner(Transform ownerTransform) {
            _owner = ownerTransform;
            // Inject into all existing CombatSteps
            foreach (var step in combatSteps) {
                step.SetOwner(_owner);
            }
        }

        // Auto-inject owner when list elements are drawn
        void InjectOwner(int index) {
            if (_owner != null && index >= 0 && index < combatSteps.Count) {
                combatSteps[index].SetOwner(_owner);
            }
        }
    }
}