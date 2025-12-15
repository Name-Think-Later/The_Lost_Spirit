using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TheLostSpirit.Infrastructure
{
    [Serializable]
    public class EventData
    {
        [ListDrawerSettings(
            ShowIndexLabels = true,
            DraggableItems = false
        )]
        [ListItemSelector(nameof(UpdateSelection))]
        public List<CombatAction> _actions = new List<CombatAction>();


        public int SelectedIndex { get; private set; }

        public bool CantInspectDuration => !_actions.Any() || SelectedIndex < 0;

        void UpdateSelection(int index) => SelectedIndex = index;
        public void ResetSelection() => SelectedIndex = -1;
    }

    [Serializable]
    public class CombatAction
    {
        [LabelWidth(60)]
        [SuffixLabel("frames", Overlay = true)]
        [MinValue(1)]
        public int DurationFrames = 30;

        [LabelWidth(60)]
        public string Name = "Effect";

        [FoldoutGroup("Details")]
        public int Damage = 10;

        [FoldoutGroup("Details")]
        public GameObject VFX;
    }
}